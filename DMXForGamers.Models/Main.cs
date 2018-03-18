using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class Main : NotifyPropertyChangedWithErrorInfoBase
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
        }

        private ObservableCollection<DMXChannel> m_Channels = new ObservableCollection<DMXChannel>();
        public ObservableCollection<DMXChannel> Channels
        {
            get { return m_Channels; }
        }

        private List<DMXProtocol> m_Protocols = new List<DMXProtocol>();
        public List<DMXProtocol> Protocols
        {
            get { return m_Protocols; }
            set { m_Protocols = value; AnnouncePropertyChanged(); }
        }

        private DMXProtocol m_SelectedProtocol;
        public DMXProtocol SelectedProtocol
        {
            get { return m_SelectedProtocol; }
            set { m_SelectedProtocol = value; AnnouncePropertyChanged(); }
        }

        private int m_SelectedMonitorIndex;
        public int SelectedMonitorIndex
        {
            get { return m_SelectedMonitorIndex; }
            set { m_SelectedMonitorIndex = value; AnnouncePropertyChanged(); }
        }

        private Queue<string> m_Output = new Queue<string>();
        public Queue<string> Output
        {
            get { return m_Output; }
            set { m_Output = value; AnnouncePropertyChanged(); }
        }

        public void TriggerOutputPropertyChanged() { OnPropertyChanged(nameof(Output)); }

        #region Monitors

        private FileMonitor m_FileMonitor = new FileMonitor();
        public FileMonitor FileMonitor
        {
            get { return m_FileMonitor; }
            set { m_FileMonitor = value; AnnouncePropertyChanged(); }
        }

        private NewProcessMonitor m_NewProcessMonitor = new NewProcessMonitor();
        public NewProcessMonitor NewProcessMonitor
        {
            get { return m_NewProcessMonitor; }
            set { m_NewProcessMonitor = value; AnnouncePropertyChanged(); }
        }

        #endregion

        private string m_EventsFile;
        public string EventsFile
        {
            get { return m_EventsFile; }
            set { m_EventsFile = value; AnnouncePropertyChanged(); }
        }

        private string m_DMXFile;
        public string DMXFile
        {
            get { return m_DMXFile; }
            set { m_DMXFile = value; AnnouncePropertyChanged(); }
        }

        private bool m_EnableRemote;
        public bool EnabledRemote
        {
            get { return m_EnableRemote; }
            set { m_EnableRemote = value; AnnouncePropertyChanged(); }
        }

        private ushort m_RemotePort;
        public ushort RemotePort
        {
            get { return m_RemotePort; }
            set { m_RemotePort = value; AnnouncePropertyChanged(); }
        }

        private ObservableCollection<EventDefinition> m_Events;
        public ObservableCollection<EventDefinition> Events
        {
            get { return m_Events; }
            set
            {
                m_Events = value;

                m_ContinuousEvents = new CollectionViewSource();
                m_ContinuousEvents.Source = Events;
                m_ContinuousEvents.Filter += (x, y) => { y.Accepted = ((EventDefinition)y.Item).Continuous; };

                m_NonContinuousEvents = new CollectionViewSource();
                m_NonContinuousEvents.Source = Events;
                m_NonContinuousEvents.Filter += (x, y) => { y.Accepted = !((EventDefinition)y.Item).Continuous; };

                AnnouncePropertyChanged();
                OnPropertyChanged(nameof(ContinuousEvents));
                OnPropertyChanged(nameof(NonContinuousEvents));
            }
        }

        private CollectionViewSource m_ContinuousEvents;
        public ICollectionView ContinuousEvents { get { return (m_ContinuousEvents == null) ? null : m_ContinuousEvents.View; } }

        private CollectionViewSource m_NonContinuousEvents;
        public ICollectionView NonContinuousEvents { get { return (m_NonContinuousEvents == null) ? null : m_NonContinuousEvents.View; } }

        #region Commands

        private ICommand _editSettings;
        public ICommand EditSettings
        {
            get { return _editSettings; }
            set { _editSettings = value; AnnouncePropertyChanged(); }
        }

        private ICommand _editEvents;
        public ICommand EditEvents
        {
            get { return _editEvents; }
            set { _editEvents = value; AnnouncePropertyChanged(); }
        }

        private ICommand _editDMXEvents;
        public ICommand EditDMXEvents
        {
            get { return _editDMXEvents; }
            set { _editDMXEvents = value; AnnouncePropertyChanged(); }
        }

        private ICommand _newEvents;
        public ICommand NewEvents
        {
            get { return _newEvents; }
            set { _newEvents = value; AnnouncePropertyChanged(); }
        }

        private ICommand _newDMXEvents;
        public ICommand NewDMXEvents
        {
            get { return _newDMXEvents; }
            set { _newDMXEvents = value; AnnouncePropertyChanged(); }
        }

        #endregion

        #region IDataErrorInfo

        public override string this[string columnName]
        {
            get
            {
                var errorStr = new StringBuilder();

                if ((columnName == nameof(EventsFile)) || (columnName == null))
                {
                    if (String.IsNullOrWhiteSpace(EventsFile) == true)
                    {
                        errorStr.AppendLine("Events File Path is required");
                    }
                }
                if ((columnName == nameof(DMXFile)) || (columnName == null))
                {
                    if (String.IsNullOrWhiteSpace(DMXFile) == true)
                    {
                        errorStr.AppendLine("DMX File Path is required");
                    }
                }

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion
    }
}
