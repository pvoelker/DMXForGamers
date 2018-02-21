using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXEvent : NotifyPropertyChangedWithErrorInfoBase
    {
        private string _eventID;
        public string EventID
        {
            get { return _eventID; }
            set { _eventID = value; AnnouncePropertyChanged(); }
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
            set
            {
                if (_timeBlocks != null)
                {
                    foreach (var item in _timeBlocks)
                    {
                        (item as DMXTimeBlock).DeleteTimeBlock = null;
                    }
                    _timeBlocks.CollectionChanged -= _timeBlocks_CollectionChanged;
                }
                _timeBlocks = value;
                if (_timeBlocks != null)
                {
                    foreach (var item in _timeBlocks)
                    {
                        (item as DMXTimeBlock).DeleteTimeBlock = new RelayCommand(x => _timeBlocks.Remove((x as DMXTimeBlock)));
                    }
                    _timeBlocks.CollectionChanged += _timeBlocks_CollectionChanged;
                }
                AnnouncePropertyChanged();
            }
        }

        private void _timeBlocks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    (item as DMXTimeBlock).DeleteTimeBlock = null;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    (item as DMXTimeBlock).DeleteTimeBlock = new RelayCommand(x => _timeBlocks.Remove((x as DMXTimeBlock)));
                }
            }
        }

        private ICommand _addTimeBlock;
        public ICommand AddTimeBlock
        {
            get { return _addTimeBlock; }
            set { _addTimeBlock = value; AnnouncePropertyChanged(); }
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
