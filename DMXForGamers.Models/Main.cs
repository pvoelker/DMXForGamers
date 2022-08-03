using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class Main : ObservableValidator
    {
        private static readonly Lazy<Main> lazy =
            new Lazy<Main>(() =>
            {
                var value = new Main();

                for (ushort i = 1; i <= 512; i++)
                {
                    value.Channels.Add(new DMXChannel(i));
                }

                return value;
            });

        public static Main Instance { get { return lazy.Value; } }

        private Main()
        {
            ValidateAllProperties();
        }

        public bool CanRun
        {
            get => !m_IsRunning && (String.IsNullOrWhiteSpace(EventsFile) == false) && (String.IsNullOrWhiteSpace(DMXFile) == false);
        }
            
        private bool m_IsRunning;
        public bool IsRunning
        {
            get => m_IsRunning;
            set
            {
                SetProperty(ref m_IsRunning, value, true);
                OnPropertyChanged(nameof(CanRun));
            }
        }

        private string m_RunningText;
        public string RunningText
        {
            get => m_RunningText;
            set => SetProperty(ref m_RunningText, value);
        }

        private string m_CopyrightInfo;
        public string CopyrightInfo
        {
            get => m_CopyrightInfo;
            set => SetProperty(ref m_CopyrightInfo, value);
        }

        private bool m_IsBusy;
        public bool IsBusy
        {
            get => m_IsBusy;
            set => SetProperty(ref m_IsBusy, value);
        }

        private ObservableCollection<DMXChannel> m_Channels = new ObservableCollection<DMXChannel>();
        public ObservableCollection<DMXChannel> Channels
        {
            get => m_Channels;
        }

        private List<DMXProtocol> m_Protocols = new List<DMXProtocol>();
        public List<DMXProtocol> Protocols
        {
            get => m_Protocols;
        }

        private DMXProtocol m_SelectedProtocol;
        public DMXProtocol SelectedProtocol
        {
            get => m_SelectedProtocol;
            set
            {
                SetProperty(ref m_SelectedProtocol, value, true);
                if(EditSettings != null)
                    EditSettings.NotifyCanExecuteChanged();
            }
        }

        private int m_SelectedMonitorIndex;
        public int SelectedMonitorIndex
        {
            get => m_SelectedMonitorIndex;
            set => SetProperty(ref m_SelectedMonitorIndex, value, true);
        }

        private Queue<string> m_Output = new Queue<string>();
        public Queue<string> Output
        {
            get => m_Output;
            set => SetProperty(ref m_Output, value, true);
        }

        public void TriggerOutputPropertyChanged() { OnPropertyChanged(nameof(Output)); }

        #region Monitors

        private FileMonitor m_FileMonitor = new FileMonitor();
        public FileMonitor FileMonitor
        {
            get => m_FileMonitor;
            set => SetProperty(ref m_FileMonitor, value, true);
        }

        private NewProcessMonitor m_NewProcessMonitor = new NewProcessMonitor();
        public NewProcessMonitor NewProcessMonitor
        {
            get => m_NewProcessMonitor;
            set => SetProperty(ref m_NewProcessMonitor, value, true);
        }

        #endregion

        private string m_EventsFile;
        [Required(ErrorMessage = "Events File Path is required")]
        public string EventsFile
        {
            get => m_EventsFile;
            set
            {
                SetProperty(ref m_EventsFile, value, true);
                OnPropertyChanged(nameof(CanRun));
                if(EditEvents != null)
                    EditEvents.NotifyCanExecuteChanged();
            }
        }

        private string m_DMXFile;
        [Required(ErrorMessage = "DMX File Path is required")]
        public string DMXFile
        {
            get => m_DMXFile;
            set
            {
                SetProperty(ref m_DMXFile, value, true);
                OnPropertyChanged(nameof(CanRun));
                if(EditDMXEvents != null)
                    EditDMXEvents.NotifyCanExecuteChanged();
            }
        }

        private bool m_EnableRemote;
        public bool EnabledRemote
        {
            get => m_EnableRemote;
            set => SetProperty(ref m_EnableRemote, value, true);
        }

        private ushort m_RemotePort = 80;
        [Range(1, ushort.MaxValue,
            ErrorMessage = "Remote Port must be greater than 0")]
        public ushort RemotePort
        {
            get => m_RemotePort;
            set => SetProperty(ref m_RemotePort, value, true);
        }

        private ObservableCollection<EventDefinition> m_Events;
        public ObservableCollection<EventDefinition> Events
        {
            get => m_Events;
            set
            {
                SetProperty(ref m_Events, value);

                m_ContinuousEvents = (m_Events == null) ? null : new ObservableCollection<EventDefinition>(m_Events.Where(x => x.Continuous == true));

                m_NonContinuousEvents = (m_Events == null) ? null : new ObservableCollection<EventDefinition>(m_Events.Where(x => x.Continuous == false));

                OnPropertyChanged(nameof(ContinuousEvents));
                OnPropertyChanged(nameof(NonContinuousEvents));
            }
        }

        private ObservableCollection<EventDefinition> m_ContinuousEvents;
        public ObservableCollection<EventDefinition> ContinuousEvents { get { return m_ContinuousEvents; } }

        private ObservableCollection<EventDefinition> m_NonContinuousEvents;
        public ObservableCollection<EventDefinition> NonContinuousEvents { get { return m_NonContinuousEvents; } }

        #region Commands

        private RelayCommand<string> _help;
        public RelayCommand<string> Help
        {
            get => _help;
            set => SetProperty(ref _help, value);
        }

        private RelayCommand _editSettings;
        public RelayCommand EditSettings
        {
            get => _editSettings;
            set => SetProperty(ref _editSettings, value);
        }

        private RelayCommand _editEvents;
        public RelayCommand EditEvents
        {
            get => _editEvents;
            set => SetProperty(ref _editEvents, value);
        }

        private RelayCommand _editDMXEvents;
        public RelayCommand EditDMXEvents
        {
            get => _editDMXEvents;
            set => SetProperty(ref _editDMXEvents, value);
        }

        private RelayCommand _newEvents;
        public RelayCommand NewEvents
        {
            get => _newEvents;
            set => SetProperty(ref _newEvents, value);
        }

        private RelayCommand _newDMXEvents;
        public RelayCommand NewDMXEvents
        {
            get => _newDMXEvents;
            set => SetProperty(ref _newDMXEvents, value);
        }

        #endregion

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(Events.SelectMany(x => x.Validate().Select(y => String.Format("Event '{0}' - {1}", x.Description, y))));

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            return errors;
        }
    }
}
