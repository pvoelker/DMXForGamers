using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Models
{
    public class Main : NotifyPropertyChangedWithErrorInfoBase
    {
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
