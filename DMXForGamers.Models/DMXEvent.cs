using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXEvent : NotifyPropertyChangedWithErrorInfoBase
    {
        public DMXEvent()
        {
            TimeBlocks = new ObservableCollection<DMXTimeBlock>();

            AddTimeBlock = new RelayCommand(x =>
            {
                TimeBlocks.Add(new DMXTimeBlock());
            });

            SortTimeBlocks = new RelayCommand(x =>
            {
                TimeBlocks = new ObservableCollection<DMXTimeBlock>(TimeBlocks.OrderBy(y => y.StartTime));
            });
        }

        private string _eventID;
        public string EventID
        {
            get { return _eventID; }
            set
            {
                _eventID = value;
                AnnouncePropertyChanged();
                OnPropertyChanged(nameof(FormattedEventID));
            }
        }
        public string FormattedEventID
        {
            get { return (String.IsNullOrWhiteSpace(EventID) == true) ? "[No ID]" : EventID; }
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

        private ICommand _sortTimeBlocks;
        public ICommand SortTimeBlocks
        {
            get { return _sortTimeBlocks; }
            set { _sortTimeBlocks = value; AnnouncePropertyChanged(); }
        }

        #region IErrorInfo

        public override string this[string columnName]
        {
            get
            {
                var errorStr = new StringBuilder();

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion

        override public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(Errors);

            #region Children

            errors.AddRange(TimeBlocks.SelectMany(x => x.Validate().
                Select(y => String.Format("Time Block ({0}ms) - {1}", x.StartTime, y))));

            #endregion

            return errors;
        }
    }
}
