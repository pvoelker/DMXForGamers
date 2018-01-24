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
                if(_events != null)
                    _events.CollectionChanged -= _events_CollectionChanged;
                _events = value;
                _events.CollectionChanged += _events_CollectionChanged;
                AnnouncePropertyChanged();
            }
        }

        private void _events_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= item_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += item_PropertyChanged;
            }
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
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
    }
}
