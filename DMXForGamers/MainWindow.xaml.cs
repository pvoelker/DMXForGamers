using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using DMXCommunication;
using DMXEngine;
using DMXForGamers.Models;

namespace DMXForGamers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DMXEngine.EventDefinitions, Models.EventDefinitions>();
                cfg.CreateMap<Models.EventDefinitions, DMXEngine.EventDefinitions>();
                cfg.CreateMap<DMXEngine.EventDefinition, Models.EventDefinition>();
                cfg.CreateMap<Models.EventDefinition, DMXEngine.EventDefinition>();

                cfg.CreateMap<DMXEngine.DMX, Models.DMXDefinitions>();
                cfg.CreateMap<Models.DMXDefinitions, DMXEngine.DMX>();
                cfg.CreateMap<DMXEngine.Event, Models.DMXEvent>();
                cfg.CreateMap<Models.DMXEvent, DMXEngine.Event>();
                cfg.CreateMap<DMXEngine.TimeBlock, Models.DMXTimeBlock>();
                cfg.CreateMap<Models.DMXTimeBlock, DMXEngine.TimeBlock>();
                cfg.CreateMap<DMXEngine.DMXValue, Models.DMXValue>();
                cfg.CreateMap<Models.DMXValue, DMXEngine.DMXValue>();

                cfg.CreateMap<DMXCommunication.DMXPortAdapter, Models.DMXProtocol>();
            });

            LoadData();

            _dmxUpdateTimer = new Timer();
            _dmxUpdateTimer.Interval = 10;
            _dmxUpdateTimer.Elapsed += DMXUpdateTimer_Elapsed;

            m_Data.EditEvents = new RelayCommand(x =>
            {
                var frm = new EditEventsWindow();
                var filedata = EventDefinitionsFile.LoadFile(m_Data.EventsFile);
                var data = Mapper.Map<DMXEngine.EventDefinitions, Models.EventDefinitions>(filedata);
                frm.DataContext = data;
                frm.ShowDialog();
            },
            x =>
            {
                return String.IsNullOrWhiteSpace(m_Data.EventsFile) == false;
            });

            m_Data.NewEvents = new RelayCommand(x =>
            {
                var frm = new EditEventsWindow();
                var data = Mapper.Map<DMXEngine.EventDefinitions, Models.EventDefinitions>(new DMXEngine.EventDefinitions());
                frm.DataContext = data;
                frm.ShowDialog();
            });

            m_Data.EditDMXEvents = new RelayCommand(x =>
            {
                var frm = new EditDMXEventsWindow();
                var filedata = DMXEventsFile.LoadFile(m_Data.DMXFile);
                var data = Mapper.Map<DMXEngine.DMX, Models.DMXDefinitions>(filedata);
                frm.DataContext = data;
                frm.ShowDialog();
            },
            x =>
            {
                return String.IsNullOrWhiteSpace(m_Data.EventsFile) == false;
            });
        }

        private Main m_Data = new Main();

        private AppSettings m_AppSettings = null;
        private string m_AppSettingsFilePath = null;

        private TextEventEngine _engine = null;
        private ITextMonitor _textMonitor = null;
        private Timer _dmxUpdateTimer = null;
        private const int MAX_LINE_COUNT = 100;

        private void LoadData()
        {
            m_AppSettingsFilePath = Environment.CurrentDirectory + "\\appsetting.xml";

            if (File.Exists(m_AppSettingsFilePath) == true)
            {
                try
                {
                    m_AppSettings = AppSettingsFile.LoadFile(m_AppSettingsFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to load to settings.\n\nDetails: " + ex.Message,
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    m_AppSettings = new AppSettings();
                }
            }
            else
            {
                m_AppSettings = new AppSettings();
                m_AppSettings.PortAdapterGuid = OpenDMX.ID;
            }
            
            var dmxPortAdapters = Mapper.Map<List<DMXCommunication.DMXPortAdapter>, List<Models.DMXProtocol>>(DMXPortAdapterHelpers.GetPortAdapters());

            m_Data.Protocols.AddRange(dmxPortAdapters);

            m_Data.SelectedProtocol = m_Data.Protocols.First(x => x.ID == m_AppSettings.PortAdapterGuid);

            m_Data.EventsFile = m_AppSettings.EventDefinitionsFilePath;
            m_Data.DMXFile = m_AppSettings.DMXFilePath;

            m_Data.FileMonitor.FilePath = m_AppSettings.MonitorFilePath;

            m_Data.NewProcessMonitor.ExeArgs = m_AppSettings.EXEArguments;
            m_Data.NewProcessMonitor.ExeFilePath = m_AppSettings.EXEFilePath;

            m_Data.SelectedMonitorIndex = m_AppSettings.TextMonitorOption;

            this.DataContext = m_Data;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            m_AppSettings.PortAdapterGuid = m_Data.SelectedProtocol.ID;

            m_AppSettings.EventDefinitionsFilePath = m_Data.EventsFile;
            m_AppSettings.DMXFilePath = m_Data.DMXFile;

            m_AppSettings.MonitorFilePath = m_Data.FileMonitor.FilePath;

            m_AppSettings.EXEArguments = m_Data.NewProcessMonitor.ExeArgs;
            m_AppSettings.EXEFilePath = m_Data.NewProcessMonitor.ExeFilePath;

            m_AppSettings.TextMonitorOption = m_Data.SelectedMonitorIndex;

            MessageBoxResult response = MessageBoxResult.Yes;
            while (response == MessageBoxResult.Yes)
            {
                try
                {
                    AppSettingsFile.SaveFile(m_AppSettings, m_AppSettingsFilePath);
                    break;
                }
                catch (Exception ex)
                {
                    response = MessageBox.Show("Unable to save settings.  Do you want to try again?  Otherwise current settings will be lost.\n\nDetails: " + ex.Message,
                        "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                }
            }
        }

        private void CheckAndKillExistingProcesses()
        {
            var existingServers = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(m_Data.NewProcessMonitor.ExeFilePath));
            if (existingServers.Length > 0)
            {
                var response = MessageBox.Show("EXE instance(s) are already running.  Do you wish to terminate them?",
                    "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (response == MessageBoxResult.Yes)
                {
                    foreach (var process in existingServers)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
            }
        }

        private void UpdateChannel(DMXChannelChange value)
        {
            if(m_Data != null)
            {
                if (value.Channel < m_Data.Channels.Count())
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new Action(() => m_Data.Channels[value.Channel - 1] = value.Value));
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _startButton.IsEnabled = false;
            _stopButton.IsEnabled = true;

            try
            {
                CheckAndKillExistingProcesses();

                IDMXCommunication dmxComm = null;
                if (m_Data.SelectedProtocol != null)
                {
                    var dmxPortAdapter = m_Data.SelectedProtocol;
                    if (dmxPortAdapter != null)
                    {
                        dmxComm = (IDMXCommunication)Activator.CreateInstance(dmxPortAdapter.Type);
                    }
                }

                if (dmxComm == null)
                {
                    MessageBox.Show("DMX Adapter/Port is Not Selected",
                        "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    var dmxEvents = DMXEventsFile.LoadFile(m_Data.DMXFile);
                    DMXStateMachine dmx = new DMXStateMachine(dmxEvents, dmxComm, UpdateChannel);

                    var eventDefs = EventDefinitionsFile.LoadFile(m_Data.EventsFile);
                    _engine = new TextEventEngine(dmx, eventDefs);

                    _dmxUpdateTimer.Enabled = true;

                    if (m_Data.SelectedMonitorIndex == 0)
                    {
                        _textMonitor = new NewProcessTextMonitor(m_Data.NewProcessMonitor.ExeFilePath, m_Data.NewProcessMonitor.ExeArgs, ProcessLine);
                    }
                    else if (m_Data.SelectedMonitorIndex == 1)
                    {
                        _textMonitor = new FileTextMonitor(m_Data.FileMonitor.FilePath, ProcessLine);
                    }
                    else if (m_Data.SelectedMonitorIndex == 2)
                    {
                        _textMonitor = null; // No text monitor, manual triggering only
                    }
                    else
                    {
                        throw new Exception("Invalid selection");
                    }
                    if (_textMonitor != null)
                        _textMonitor.Start();

                    var events = new List<Models.EventDefinition>();
                    foreach (var item in _engine.EventDefinitions.Events)
                    {
                        events.Add(new Models.EventDefinition()
                        {
                            Description = item.Description,
                            EventID = item.EventID,
                            Continuous = item.Continuous,
                            EventOn = new RelayCommand(x =>
                            {
                                _engine.ManualAddEvent((string)x, item.Continuous);
                            }),
                            EventOff = new RelayCommand(x =>
                            {
                                _engine.ManualRemoveEvent((string)x);
                            })
                        });
                    }

                    m_Data.Events = new ObservableCollection<Models.EventDefinition>(events);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception occured.\n\nDetails: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                StopButton_Click(this, null);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _startButton.IsEnabled = true;
            _stopButton.IsEnabled = false;

            if (_dmxUpdateTimer != null)
            {
                _dmxUpdateTimer.Enabled = false;
            }

            if (_engine != null)
            {
                _engine.Dispose();
                _engine = null;
            }

            if (_textMonitor != null)
            {
                _textMonitor.Dispose();
                _textMonitor = null;
            }
        }

        private void ProcessLine(string line)
        {
            if (line != null)
            {
                lock (m_Data.Output)
                {
                    m_Data.Output.Enqueue(line);
                    if (m_Data.Output.Count() > MAX_LINE_COUNT)
                    {
                        m_Data.Output.Dequeue();
                    }
                }
                m_Data.TriggerOutputPropertyChanged();

                Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() => m_OutputScroll.ScrollToEnd()));

                ProcessDMXEvent(line);
            }
        }

        private void ProcessDMXEvent(string line)
        {
            if (_engine != null)
            {
                _engine.ProcessText(line);
            }
        }

        void DMXUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _dmxUpdateTimer.Enabled = false;
            try
            {
                if (_engine != null)
                {
                    _engine.Execute(DateTime.Now);
                }
            }
            finally
            {
                _dmxUpdateTimer.Enabled = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopButton_Click(this, null);
        }
    }
}
