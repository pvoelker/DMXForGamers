using DMXCommunication;
using DMXEngine;
using DMXForGamers.Models;
using DMXForGamers.Web;
using CommunityToolkit.Mvvm.Input;
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

            LoadData();

            _dmxUpdateTimer = new Timer();
            _dmxUpdateTimer.Interval = 10;
            _dmxUpdateTimer.Elapsed += DMXUpdateTimer_Elapsed;

            m_Data.Help = new RelayCommand<string>((x) =>
            {
                var helpTopic = x as string;

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var chmPath = Path.Combine(path, "DMXForGamersHelp.chm");
                if(helpTopic == null)
                    System.Windows.Forms.Help.ShowHelp(null, chmPath);
                else
                    System.Windows.Forms.Help.ShowHelp(null, chmPath, System.Windows.Forms.HelpNavigator.TopicId, helpTopic);
            });

            m_Data.EditSettings = new RelayCommand(() =>
            {
                var frm = new SettingsWindow();
                frm.DataContext = m_Data.SelectedProtocol.Settings;
                frm.ShowDialog();
            },
            () =>
            {
                return (m_Data.SelectedProtocol == null) ? false : (m_Data.SelectedProtocol.Settings != null);
            });

            m_Data.EditEvents = new RelayCommand(() =>
            {
                if (File.Exists(m_Data.EventsFile) == false)
                {
                    MessageBox.Show("Unable to load: " + m_Data.EventsFile, "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var fileData = EventDefinitionsFile.LoadFile(m_Data.EventsFile);
                    CreateOrEditEvents(m_Data.EventsFile, fileData);
                }
            },
            () =>
            {
                return String.IsNullOrWhiteSpace(m_Data.EventsFile) == false;
            });

            m_Data.NewEvents = new RelayCommand(() =>
            {
                CreateOrEditEvents(m_Data.EventsFile, new DMXEngine.EventDefinitions());
            });

            m_Data.EditDMXEvents = new RelayCommand(() =>
            {
                if (File.Exists(m_Data.DMXFile) == false)
                {
                    MessageBox.Show("Unable to load: " + m_Data.DMXFile, "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var fileData = DMXEventsFile.LoadFile(m_Data.DMXFile);
                    CreateOrEditDMXEvents(m_Data.DMXFile, fileData);
                }
            },
            () =>
            {
                return String.IsNullOrWhiteSpace(m_Data.DMXFile) == false;
            });

            m_Data.NewDMXEvents = new RelayCommand(() =>
            {
                CreateOrEditDMXEvents(m_Data.DMXFile, new DMXEngine.DMX());
            });

#if DEBUG
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in assemblies)
            {
                Console.WriteLine($"Assembly Info --- {item.GetName()}");
            }
#endif
        }

        private Main m_Data = Main.Instance;

        private AppSettings m_AppSettings = null;
        private string m_AppSettingsFilePath = null;

        private TextEventEngine _engine = null;
        private ITextMonitor _textMonitor = null;
        private Timer _dmxUpdateTimer = null;
        private const int MAX_LINE_COUNT = 100;

        private SelfHost _webHost = null;

        private void LoadData()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDataPath = Path.Combine(appDataPath, "DMX for Gamers");
            if (Directory.Exists(appDataPath) == false)
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

            var dmxPortAdapters = new List<DMXProtocol>();
            var mapper = new Mappers.DMXProtocol();
            foreach (var item in DMXPortAdapterHelpers.GetPortAdapters())
            {
                dmxPortAdapters.Add(mapper.ToModel(item));
            }

            m_Data.Protocols.AddRange(dmxPortAdapters);

            m_Data.SelectedProtocol = m_Data.Protocols.FirstOrDefault(x => x.ID == m_AppSettings.PortAdapterGuid);
            if (m_Data.SelectedProtocol != null)
            {
                m_Data.SelectedProtocol.Settings = m_AppSettings.PortAdapterConfig;
            }

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
            if (m_Data.SelectedProtocol != null)
            {
                m_AppSettings.PortAdapterGuid = m_Data.SelectedProtocol.ID;
                m_AppSettings.PortAdapterConfig = m_Data.SelectedProtocol.Settings as BaseSettings;
            }

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

        private void UpdateEvent(EventChange value)
        {
            if (m_Data != null)
            {
                var foundEvent = m_Data.Events.SingleOrDefault(x => x.EventID == value.EventID);
                if (foundEvent != null)
                {
                    foundEvent.State = value.State;
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            m_Data.IsRunning = true;

            try
            {
                m_Data.IsBusy = true;

                // PEV - 3/21/2018 - Wait for UI to update and 'busy' to show...
                this.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);

                if (m_Data.SelectedMonitorIndex == 0)
                {
                    CheckAndKillExistingProcesses();
                }

                IDMXCommunication dmxComm = null;
                if (m_Data.SelectedProtocol != null)
                {
                    var dmxPortAdapter = m_Data.SelectedProtocol;
                    if (dmxPortAdapter != null)
                    {
                        dmxComm = (IDMXCommunication)Activator.CreateInstance(dmxPortAdapter.Type);
                    }
                }

                if (File.Exists(m_Data.DMXFile) == false)
                {
                    MessageBox.Show(String.Format("DMX file ('{0}') does not exist", m_Data.DMXFile),
                        "Unable to Start", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    StopButton_Click(this, null);
                }
                else if (File.Exists(m_Data.EventsFile) == false)
                {
                    MessageBox.Show(String.Format("Event file ('{0}') does not exist", m_Data.EventsFile),
                        "Unable to Start", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    StopButton_Click(this, null);
                }
                else if (dmxComm == null)
                {
                    MessageBox.Show("DMX Adapter/Port is Not Selected",
                        "Unable to Start", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    StopButton_Click(this, null);
                }
                else if ((m_Data.EnabledRemote == true) && (m_Data.RemotePort == 0))
                {
                    MessageBox.Show("Remote Port must be greater than 0",
                        "Unable to Start", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    StopButton_Click(this, null);
                }
                else
                {
                    var dmxEvents = DMXEventsFile.LoadFile(m_Data.DMXFile);
                    DMXStateMachine dmx = new DMXStateMachine(dmxEvents, dmxComm, UpdateChannel, UpdateEvent);

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
                            EventOn = new RelayCommand(() =>
                            {
                                _engine.ManualAddEvent(item.EventID, item.Continuous);
                            }),
                            EventOff = new RelayCommand(() =>
                            {
                                _engine.ManualRemoveEvent(item.EventID);
                            })
                        });
                    }

                    m_Data.Events = new ObservableCollection<Models.EventDefinition>(events);

                    if (m_Data.EnabledRemote == true)
                    {
                        try
                        {
                            _webHost = new SelfHost();
                            _webHost.Start(m_Data.RemotePort).Wait();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Unable to Start Web Server", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        m_Data.RunningText = "Remote Control: http://" + string.Join(",", GetLocalIPs()) + ":" + m_Data.RemotePort;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#else
                MessageBox.Show("Unhandled exception occured.\n\nDetails: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                StopButton_Click(this, null);
#endif
            }
            finally
            {
                m_Data.IsBusy = false;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_webHost != null)
            {
                _webHost.Dispose();
                _webHost = null;
            }

            m_Data.IsRunning = false;

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
            var mapper = new Mappers.EventDefinitions();
            frm.DataContext = mapper.ToModel(fileData);
            frm.ShowDialog();
            if (frm.IsSave == true)
            {
                fileData = mapper.FromModel(frm.DataContext as Models.EventDefinitions);
                EventDefinitionsFile.SaveFile(fileData, fileName);
            }
        }

        private static void CreateOrEditDMXEvents(string fileName, DMXEngine.DMX fileData)
        {
            var frm = new EditDMXEventsWindow();
            var mapper = new Mappers.DMXDefinitions();
            frm.DataContext = mapper.ToModel(fileData);
            frm.ShowDialog();
            if (frm.IsSave == true)
            {
                fileData = mapper.FromModel(frm.DataContext as DMXDefinitions);
                DMXEventsFile.SaveFile(fileData, fileName);
            }
        }

        private static IEnumerable<string> GetLocalIPs()
        {
            var localIPs = Dns.GetHostAddresses(Dns.GetHostName());

            var retVal = new List<string>();

            retVal.AddRange(localIPs.Where(x => x.AddressFamily == AddressFamily.InterNetwork).Select(x => x.ToString()));

            if (retVal.Count > 0)
            {
                return retVal;
            }

            retVal.AddRange(localIPs.Where(x => x.AddressFamily == AddressFamily.InterNetworkV6).Select(x => x.ToString()));

            if (retVal.Count > 0)
            {
                return retVal;
            }

            return null;
        }
    }
}
