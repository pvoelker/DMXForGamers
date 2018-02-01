using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXEvent : NotifyPropertyChangedWithErrorInfoBase
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; AnnouncePropertyChanged(); }
        }

        private int _timeSpan;
        public int TimeSpan
        {
            get { return _timeSpan; }
            set { _timeSpan = value; AnnouncePropertyChanged(); }
        }

        private int _repeatCount = 1;
        public int RepeatCount
        {
            get { return _repeatCount; }
            set { _repeatCount = value; AnnouncePropertyChanged(); }
        }

        private ObservableCollection<DMXTimeBlock> _timeBlocks;
        public ObservableCollection<DMXTimeBlock> TimeBlocks
        {
            get { return _timeBlocks; }
            set { _timeBlocks = value; AnnouncePropertyChanged(); }
        }

        private ICommand _addEvent;
        public ICommand AddEvent
        {
            get { return _addEvent; }
            set { _addEvent = value; AnnouncePropertyChanged(); }
        }

        private ICommand _deleteEvent;
        public ICommand DeleteEvent
        {
            get { return _deleteEvent; }
            set { _deleteEvent = value; AnnouncePropertyChanged(); }
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
