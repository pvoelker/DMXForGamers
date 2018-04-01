using AutoMapper;
using DMXCommunication;
using DMXEngine;
using DMXForGamers.Models;
using DMXForGamers.Web;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

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
                cfg.CreateMap<DMXEngine.EventDefinitions, Models.EventDefinitions>().
                    ForMember(x => x.AddEvent, opt => opt.Ignore());
                cfg.CreateMap<Models.EventDefinitions, DMXEngine.EventDefinitions>();
                cfg.CreateMap<DMXEngine.EventDefinition, Models.EventDefinition>().
                    ForMember(x => x.DeleteEvent, opt => opt.Ignore()).
                    ForMember(x => x.EventOff, opt => opt.Ignore()).
                    ForMember(x => x.EventOn, opt => opt.Ignore());
                cfg.CreateMap<Models.EventDefinition, DMXEngine.EventDefinition>();

                cfg.CreateMap<DMXEngine.DMX, Models.DMXDefinitions>().
                    ForMember(x => x.AddBaseValue, opt => opt.Ignore()).
                    ForMember(x => x.AddEvent, opt => opt.Ignore());
                cfg.CreateMap<Models.DMXDefinitions, DMXEngine.DMX>();
                cfg.CreateMap<DMXEngine.Event, Models.DMXEvent>().
                    ForMember(x => x.AddTimeBlock, opt => opt.Ignore()).
                    ForMember(x => x.DeleteEvent, opt => opt.Ignore());
                cfg.CreateMap<Models.DMXEvent, DMXEngine.Event>();
                cfg.CreateMap<DMXEngine.TimeBlock, Models.DMXTimeBlock>().
                    ForMember(x => x.AddDMXValue, opt => opt.Ignore()).
                    ForMember(x => x.DeleteTimeBlock, opt => opt.Ignore());
                cfg.CreateMap<Models.DMXTimeBlock, DMXEngine.TimeBlock>();
                cfg.CreateMap<DMXEngine.DMXValue, Models.DMXValue>().
                    ForMember(x => x.DeleteDMXValue, opt => opt.Ignore());
                cfg.CreateMap<Models.DMXValue, DMXEngine.DMXValue>();

                cfg.CreateMap<DMXCommunication.DMXPortAdapter, Models.DMXProtocol>();
            });

            Mapper.AssertConfigurationIsValid();

            LoadData();

            _dmxUpdateTimer = new Timer();
            _dmxUpdateTimer.Interval = 10;
            _dmxUpdateTimer.Elapsed += DMXUpdateTimer_Elapsed;

            m_Data.EditSettings = new RelayCommand(x =>
            {
                var frm = new SettingsWindow();
                frm.DataContext = m_Data.SelectedProtocol.Settings;
                frm.ShowDialog();
            },
            x =>
            {
                return (m_Data.SelectedProtocol == null) ? false : (m_Data.SelectedProtocol.Settings != null);
            });

            m_Data.EditEvents = new RelayCommand(x =>
            {
                var fileData = EventDefinitionsFile.LoadFile(m_Data.EventsFile);
                CreateOrEditEvents(m_Data.EventsFile, fileData);
            },
            x =>
            {
                return String.IsNullOrWhiteSpace(m_Data.EventsFile) == false;
            });

            m_Data.NewEvents = new RelayCommand(x =>
            {
                CreateOrEditEvents(m_Data.EventsFile, new DMXEngine.EventDefinitions());
            });

            m_Data.EditDMXEvents = new RelayCommand(x =>
            {
                var fileData = DMXEventsFile.LoadFile(m_Data.DMXFile);
                CreateOrEditDMXEvents(m_Data.DMXFile, fileData);
            },
            x =>
            {
                return String.IsNullOrWhiteSpace(m_Data.EventsFile) == false;
            });

            m_Data.NewDMXEvents = new RelayCommand(x =>
            {
                CreateOrEditDMXEvents(m_Data.DMXFile, new DMXEngine.DMX());
            });

            _stopButton.IsEnabled = false;
        }

        private Main m_Data = Main.Instance;

        private AppSettings m_AppSettings = null;
        private string m_AppSettingsFilePath = null;

        private TextEventEngine _engine = null;
        private ITextMonitor _textMonitor = null;
        private Timer _dmxUpdateTimer = null;
        private const int MAX_LINE_COUNT = 100;

        private INancyBootstrapper _webHostBootstrapper = null;
        private NancyHost _webHost = null;

        private void LoadData()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDataPath = Path.Combine(appDataPath, "DMX for Gamers");
            if(Directory.Exists(appDataPath) == false)
            {
                Directory.CreateDirectory(appDataPath);
            }

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            m_Data.CopyrightInfo = versionInfo.LegalCopyright;

            m_AppSettingsFilePath = Path.Combine(appDataPath, "appsetting.xml");

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

            m_Data.SelectedProtocol = m_Data.Protocols.FirstOrDefault(x => x.ID == m_AppSettings.PortAdapterGuid);

            m_Data.EventsFile = m_AppSettings.EventDefinitionsFilePath;
            m_Data.DMXFile = m_AppSettings.DMXFilePath;

            m_Data.FileMonitor.FilePath = m_AppSettings.MonitorFilePath;

            m_Data.NewProcessMonitor.ExeArgs = m_AppSettings.EXEArguments;
            m_Data.NewProcessMonitor.ExeFilePath = m_AppSettings.EXEFilePath;

            m_Data.SelectedMonitorIndex = m_AppSettings.TextMonitorOption;

            m_Data.EnabledRemote = m_AppSettings.EnableRemoteControl;
            m_Data.RemotePort = m_AppSettings.RemoteControlPort;

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

            m_AppSettings.EnableRemoteControl = m_Data.EnabledRemote;
            m_AppSettings.RemoteControlPort = m_Data.RemotePort;

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
            if (m_Data != null)
            {
                if (value.Channel < m_Data.Channels.Count())
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            var data = m_Data.Channels[value.Channel - 1];
                            data.Value = value.Value;
                        }));
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _startButton.IsEnabled = false;
            _stopButton.IsEnabled = true;

            try
            {
                m_Data.IsBusy = true;

                // PEV - 3/21/2018 - Wait for UI to update and 'busy' to show...
                this.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);

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

                    var hostConfig = new Nancy.Hosting.Self.HostConfiguration();
                    hostConfig.UrlReservations.CreateAutomatically = true;

                    if (m_Data.EnabledRemote == true)
                    {
                        try
                        {
                            _webHostBootstrapper = new CustomBootstrapper();

                            _webHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:" + m_Data.RemotePort), _webHostBootstrapper, hostConfig);
                            _webHost.Start();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Unable to Start Web Server", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        _progressStatus.Text = "Remote Control: http://" + GetLocalIP() + ":" + m_Data.RemotePort;
                    }

                    _progressBar.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled exception occured.\n\nDetails: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                StopButton_Click(this, null);
            }
            finally
            {
                m_Data.IsBusy = false;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _progressBar.Visibility = Visibility.Hidden;
            _progressStatus.Text = String.Empty;

            if (_webHost != null)
            {
                _webHost.Dispose();
                _webHost = null;
            }

            if(_webHostBootstrapper != null)
            {
                _webHostBootstrapper.Dispose();
                _webHostBootstrapper = null;
            }

            _startButton.IsEnabled = true;
            _stopButton.IsEnabled = false;

            m_Data.Events = null;

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

        private static void CreateOrEditEvents(string fileName, DMXEngine.EventDefinitions fileData)
        {
            var frm = new EditEventsWindow();
            var data = Mapper.Map<DMXEngine.EventDefinitions, Models.EventDefinitions>(fileData);
            frm.DataContext = data;
            frm.ShowDialog();
            if (frm.IsSave == true)
            {
                fileData = Mapper.Map<Models.EventDefinitions, DMXEngine.EventDefinitions>(data);
                EventDefinitionsFile.SaveFile(fileData, fileName);
            }
        }

        private static void CreateOrEditDMXEvents(string fileName, DMXEngine.DMX fileData)
        {
            var frm = new EditDMXEventsWindow();
            var data = Mapper.Map<DMXEngine.DMX, Models.DMXDefinitions>(fileData);
            frm.DataContext = data;
            frm.ShowDialog();
            if (frm.IsSave == true)
            {
                fileData = Mapper.Map<Models.DMXDefinitions, DMXEngine.DMX>(data);
                DMXEventsFile.SaveFile(fileData, fileName);
            }
        }

        private static string GetLocalIP()
        {
            var localIPs = Dns.GetHostAddresses(Dns.GetHostName());

            var addr = localIPs.SingleOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

            if(addr != null)
            {
                return addr.ToString();
            }

            addr = localIPs.SingleOrDefault(x => x.AddressFamily == AddressFamily.InterNetworkV6);

            if (addr != null)
            {
                return addr.ToString();
            }

            return null;
        }
    }
}
