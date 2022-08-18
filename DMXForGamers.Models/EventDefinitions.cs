using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class EventDefinitions : ObservableValidator
    {
        public EventDefinitions()
        {
            Events.CollectionChanged += Events_CollectionChanged;

            AddEvent = new RelayCommand(() =>
            {
                Events.Add(new EventDefinition());
            });

            ValidateAllProperties();
        }

        private void Events_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.Cast<EventDefinition>();
            var newItems = e.NewItems?.Cast<EventDefinition>();

            if (oldItems != null)
            {
                foreach (var item in oldItems)
                {
                    item.DeleteEvent = null;
                    item.ParentCollection = null;
                }
            }

            if (newItems != null)
            {
                foreach (var item in newItems)
                {
                    item.DeleteEvent = new RelayCommand(() => Events.Remove(item));
                    item.ParentCollection = Events;
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value, true); }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value, true); }
        }

        private DeepObservableCollection<EventDefinition> _events = new DeepObservableCollection<EventDefinition>(new List<string>
        {
            nameof(EventDefinition.DeleteEvent),
            nameof(EventDefinition.ParentCollection)
        });
        public DeepObservableCollection<EventDefinition> Events
        {
            get { return _events; }
        }

        private ICommand _addEvent;
        public ICommand AddEvent
        {
            get { return _addEvent; }
            set { SetProperty(ref _addEvent, value);  }
        }

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            #region Children

            errors.AddRange(Events.SelectMany(x => x.Validate().
                Select(y => String.Format("Event '{0}' - {1}", x.FormattedEventID, y))));

            #endregion

            return errors;
        }
    }
}
