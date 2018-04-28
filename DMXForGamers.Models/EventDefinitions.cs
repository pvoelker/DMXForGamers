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

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; AnnouncePropertyChanged(); }
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
                        var eventDefinition = (item as EventDefinition);
                        eventDefinition.DeleteEvent = null;
                        eventDefinition.ParentCollection = null;

                    }
                    _events.CollectionChanged -= _events_CollectionChanged;
                }
                _events = value;
                if (_events != null)
                {
                    foreach (var item in _events)
                    {
                        var eventDefinition = (item as EventDefinition);
                        eventDefinition.DeleteEvent = new RelayCommand(x => _events.Remove((x as EventDefinition)));
                        eventDefinition.ParentCollection = Events;

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
                    var eventDefinition = (item as EventDefinition);
                    eventDefinition.DeleteEvent = null;
                    eventDefinition.ParentCollection = null;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var eventDefinition = (item as EventDefinition);
                    eventDefinition.DeleteEvent = new RelayCommand(x => _events.Remove((x as EventDefinition)));
                    eventDefinition.ParentCollection = Events;
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

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion

        override public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(Errors);

            #region Children

            errors.AddRange(Events.SelectMany(x => x.Validate().
                Select(y => String.Format("Event '{0}' - {1}", x.FormattedEventID, y))));

            #endregion

            return errors;
        }
    }
}
