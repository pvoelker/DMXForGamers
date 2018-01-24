using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXDefinitions : NotifyPropertyChangedWithErrorInfoBase
    {
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; AnnouncePropertyChanged(); }
        }

        private ObservableCollection<DMXValue> _baseDMXValues;
        public ObservableCollection<DMXValue> BaseDMXValues
        {
            get { return _baseDMXValues; }
            set { _baseDMXValues = value; AnnouncePropertyChanged(); }
        }

        private ObservableCollection<DMXEvent> _events;
        public ObservableCollection<DMXEvent> Events
        {
            get { return _events; }
            set { _events = value; AnnouncePropertyChanged(); }
        }

        private ICommand _addEvent;
        public ICommand AddEvent
        {
            get { return _addEvent; }
            set { _addEvent = value; AnnouncePropertyChanged(); }
        }

        #region IErrorInfo

        public override string this[string columnName]
        {
            get
            {
                var errorStr = new StringBuilder();

                //if ((columnName == nameof(EventID)) || (columnName == null))
                //{
                //    if (String.IsNullOrWhiteSpace(EventID) == true)
                //    {
                //        errorStr.AppendLine("Event ID is required");
                //    }
                //}
                //if ((columnName == nameof(Pattern)) || (columnName == null))
                //{
                //    if (UseRegEx == true)
                //    {
                //        if (String.IsNullOrWhiteSpace(Pattern) == true)
                //        {
                //            errorStr.AppendLine("Matching Pattern is required or disable Regular Expression option");
                //        }
                //    }
                //}

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion
    }
}
