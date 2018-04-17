using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class EventDefinitions : NotifyPropertyChangedWithErrorInfoBase
    {
        public EventDefinitions()
        {
            Events = new ObservableCollection<EventDefinition>();

            AddEvent = new RelayCommand(x =>
            {
                Events.Add(new EventDefinition());
            });
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; AnnouncePropertyChanged(); }
        }

        private ObservableCollection<EventDefinition> _events;
        public ObservableCollection<EventDefinition> Events
        {
            get { return _events; }
            set
            {
                if (_events != null)
                {
                    foreach (var item in _events)
                    {
                        (item as EventDefinition).DeleteEvent = null;
                    }
                    _events.CollectionChanged -= _events_CollectionChanged;
                }
                _events = value;
                if (_events != null)
                {
                    foreach (var item in _events)
                    {
                        (item as EventDefinition).DeleteEvent = new RelayCommand(x => _events.Remove((x as EventDefinition)));
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
                    (item as EventDefinition).DeleteEvent = null;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    (item as EventDefinition).DeleteEvent = new RelayCommand(x => _events.Remove((x as EventDefinition)));
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

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion

        override public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(Errors);

            var duplicateEventIDs = Events.GroupBy(x => x.EventID.ToUpper()).Where(y => y.Count() > 1).Select(z => z.Key);

            errors.AddRange(duplicateEventIDs.Select(x => String.Format("Event ID '{0}' is used more than once", x)));

            #region Children

            errors.AddRange(Events.SelectMany(x => x.Validate().
                Select(y => String.Format("Event '{0}' - {1}", x.FormattedEventID, y))));

            #endregion

            return errors;
        }
    }
}
