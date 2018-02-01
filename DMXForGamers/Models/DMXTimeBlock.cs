using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXTimeBlock : NotifyPropertyChangedWithErrorInfoBase
    {
        private int _startTime;
        public int StartTime
        {
            get { return _startTime; }
            set { _startTime = value; AnnouncePropertyChanged(); }
        }

        private int _timeSpan;
        public int TimeSpan
        {
            get { return _timeSpan; }
            set { _timeSpan = value; AnnouncePropertyChanged(); }
        }

        private ObservableCollection<DMXValue> _dmxValues;
        public ObservableCollection<DMXValue> DMXValues
        {
            get { return _dmxValues; }
            set { _dmxValues = value; AnnouncePropertyChanged(); }
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
