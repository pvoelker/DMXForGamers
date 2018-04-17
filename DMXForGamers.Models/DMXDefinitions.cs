using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXDefinitions : NotifyPropertyChangedWithErrorInfoBase
    {
        public DMXDefinitions()
        {
            BaseDMXValues = new ObservableCollection<DMXValue>();
            Events = new ObservableCollection<DMXEvent>();

            AddBaseValue = new RelayCommand(x =>
            {
                BaseDMXValues.Add(new DMXValue());
            });

            AddEvent = new RelayCommand(x =>
            {
                Events.Add(new DMXEvent());
            });

            SortBaseDMXValues = new RelayCommand(x =>
            {
                BaseDMXValues = new ObservableCollection<DMXValue>(BaseDMXValues.OrderBy(y => y.Channel));
            });
        }

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
                OnPropertyChanged(nameof(UsedBaseChannels));
            }
        }

        public IEnumerable<ushort> UsedBaseChannels { get { return BaseDMXValues.Select(x => x.Channel).Distinct(); } }

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
                OnPropertyChanged(nameof(UsedEventChannels));
            }
        }

        public IEnumerable<ushort> UsedEventChannels { get { return Events.SelectMany(x => x.TimeBlocks).SelectMany(x => x.DMXValues).Select(x => x.Channel).Distinct(); } }

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

        private ICommand _addBaseValue;
        public ICommand AddBaseValue
        {
            get { return _addBaseValue; }
            set { _addBaseValue = value; AnnouncePropertyChanged(); }
        }

        private ICommand _addEvent;
        public ICommand AddEvent
        {
            get { return _addEvent; }
            set { _addEvent = value; AnnouncePropertyChanged(); }
        }

        private ICommand _sortBaseDMXValues;
        public ICommand SortBaseDMXValues
        {
            get { return _sortBaseDMXValues; }
            set { _sortBaseDMXValues = value; AnnouncePropertyChanged(); }
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

            var duplicateEventIDs = Events.GroupBy(x => x.EventID.ToUpper()).Where(y => y.Count() > 1).Select(z => z.Key);
            errors.AddRange(duplicateEventIDs.Select(x => String.Format("Event ID '{0}' is used more than once", x)));

            if (BaseDMXValues.Count == 0)
            {
                errors.Add("No Base DMX Value are defined");
            }

            if (Events.Count == 0)
            {
                errors.Add("No Events are defined");
            }

            var usedBaseChannels = UsedBaseChannels;
            var usedEventChannels = UsedEventChannels;

            var undefinedBaseChannels = usedEventChannels.Except(usedBaseChannels);

            if (undefinedBaseChannels.Count() > 0)
            {
                errors.Add("Base DXM Channels have not been defined for the following channels used in events: " + String.Join(",", undefinedBaseChannels));
            }

            var duplicateDMXChannels = BaseDMXValues.GroupBy(x => x.Channel).Where(y => y.Count() > 1).Select(z => z.Key);
            errors.AddRange(duplicateDMXChannels.Select(x => String.Format("DMX Channel '{0}' is defined more than once", x)));

            errors.AddRange(BaseDMXValues.SelectMany(x => x.Validate().
                Select(y => String.Format("Base DMX Channel {0} - {1}", x.Channel, y))));

            errors.AddRange(Events.SelectMany(x => x.Validate().
                Select(y => String.Format("Event '{0}' - {1}", x.FormattedEventID, y))));

            #endregion

            return errors;
        }
    }
}
