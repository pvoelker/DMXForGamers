using DMXForGamers.Models.Extensions;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXDefinitions : ObservableValidator
    {
        public DMXDefinitions()
        {
            BaseDMXValues.CollectionChanged += BaseDMXValues_CollectionChanged;
            Events.CollectionChanged += Events_CollectionChanged;

            AddBaseValue = new RelayCommand(() =>
            {
                BaseDMXValues.Add(new DMXValue());
            });

            AddEvent = new RelayCommand(() =>
            {
                Events.Add(new DMXEvent());
            });

            SortBaseDMXValues = new RelayCommand(() =>
            {
                BaseDMXValues.Sort((x, y) => x.Channel - y.Channel);
            });

            ValidateAllProperties();
        }

        private void BaseDMXValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.Cast<DMXValue>();
            var newItems = e.NewItems?.Cast<DMXValue>();

            if (oldItems != null)
            {
                foreach (var item in oldItems)
                {
                    item.DeleteDMXValue = null;
                    item.ParentCollection = null;
                }
            }

            if (newItems != null)
            {
                foreach (var item in newItems)
                {
                    item.DeleteDMXValue = new RelayCommand(() => BaseDMXValues.Remove(item));
                    item.ParentCollection = BaseDMXValues;
                }
            }
        }

        private void Events_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.Cast<DMXEvent>();
            var newItems = e.NewItems?.Cast<DMXEvent>();

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
            get => _description;
            set => SetProperty(ref _description, value, true);
        }

        private bool _allowOneActiveEvent;
        public bool AllowOneActiveEvent
        {
            get => _allowOneActiveEvent;
            set => SetProperty(ref _allowOneActiveEvent, value, true);
        }

        private DeepObservableCollection<DMXValue> _baseDMXValues = new DeepObservableCollection<DMXValue>(new List<string> { nameof(DMXValue.DeleteDMXValue), nameof(DMXValue.ParentCollection) });
        public DeepObservableCollection<DMXValue> BaseDMXValues
        {
            get { return _baseDMXValues; }
        }

        public IEnumerable<ushort> UsedBaseChannels { get { return BaseDMXValues.Select(x => x.Channel).Distinct(); } }

        private DeepObservableCollection<DMXEvent> _events = new DeepObservableCollection<DMXEvent>(new List<string> { nameof(DMXEvent.DeleteEvent), nameof(DMXValue.ParentCollection) });
        public DeepObservableCollection<DMXEvent> Events
        {
            get { return _events; }
        }

        public IEnumerable<ushort> UsedEventChannels { get { return Events.SelectMany(x => x.TimeBlocks).SelectMany(x => x.DMXValues).Select(x => x.Channel).Distinct(); } }

        private ICommand _addBaseValue;
        public ICommand AddBaseValue
        {
            get => _addBaseValue;
            set => SetProperty(ref _addBaseValue, value, nameof(AddBaseValue));
        }

        private ICommand _addEvent;
        public ICommand AddEvent
        {
            get => _addEvent;
            set => SetProperty(ref _addEvent, value, nameof(AddEvent));
        }

        private ICommand _sortBaseDMXValues;
        public ICommand SortBaseDMXValues
        {
            get => _sortBaseDMXValues;
            set => SetProperty(ref _sortBaseDMXValues, value, nameof(SortBaseDMXValues));
        }

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            #region Children

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

            errors.AddRange(BaseDMXValues.SelectMany(x => x.Validate().
                Select(y => String.Format("Base DMX Channel {0} - {1}", x.Channel, y))));

            errors.AddRange(Events.SelectMany(x => x.Validate().
                Select(y => String.Format("Event '{0}' - {1}", x.FormattedEventID, y))));

            #endregion

            return errors;
        }
    }
}
