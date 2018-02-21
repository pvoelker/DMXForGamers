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
            set
            {
                if (_baseDMXValues != null)
                {
                    foreach (var item in _baseDMXValues)
                    {
                        (item as DMXValue).DeleteDMXValue = null;
                    }
                    _baseDMXValues.CollectionChanged -= _baseDMXValues_CollectionChanged;
                }
                _baseDMXValues = value;
                if (_baseDMXValues != null)
                {
                    foreach (var item in _baseDMXValues)
                    {
                        (item as DMXValue).DeleteDMXValue = new RelayCommand(x => _baseDMXValues.Remove((x as DMXValue)));
                    }
                    _baseDMXValues.CollectionChanged += _baseDMXValues_CollectionChanged;
                }
                AnnouncePropertyChanged();
            }
        }

        private void _baseDMXValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    (item as DMXValue).DeleteDMXValue = null;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    (item as DMXValue).DeleteDMXValue = new RelayCommand(x => _baseDMXValues.Remove((x as DMXValue)));
                }
            }
        }

        private ObservableCollection<DMXEvent> _events;
        public ObservableCollection<DMXEvent> Events
        {
            get { return _events; }
            set
            {
                if (_events != null)
                {
                    foreach (var item in _events)
                    {
                        (item as DMXEvent).DeleteEvent = null;
                    }
                    _events.CollectionChanged -= _events_CollectionChanged;
                }
                _events = value;
                if (_events != null)
                {
                    foreach (var item in _events)
                    {
                        (item as DMXEvent).DeleteEvent = new RelayCommand(x => _events.Remove((x as DMXEvent)));
                    }
                    _events.CollectionChanged += _events_CollectionChanged;
                }
                AnnouncePropertyChanged();
            }
        }

        private void _events_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    (item as DMXEvent).DeleteEvent = null;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    (item as DMXEvent).DeleteEvent = new RelayCommand(x => _events.Remove((x as DMXEvent)));
                }
            }
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
