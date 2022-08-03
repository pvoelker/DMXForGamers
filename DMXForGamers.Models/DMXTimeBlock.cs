using DMXForGamers.Models.Extensions;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXTimeBlock : ObservableValidator
    {
        public DMXTimeBlock()
        {
            DMXValues.CollectionChanged += DMXValues_CollectionChanged;

            AddDMXValue = new RelayCommand(() =>
            {
                ushort newChannel = 1;
                if (DMXValues.Count > 0)
                {
                    newChannel = DMXValues.Max(x2 => x2.Channel);
                    newChannel++;
                }

                DMXValues.Add(new DMXValue { Channel = newChannel });
            });

            SortDMXValues = new RelayCommand(() =>
            {
                DMXValues.Sort((x, y) => x.Channel - y.Channel);
            });

            ValidateAllProperties();
        }

        private void DMXValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
                    item.DeleteDMXValue = new RelayCommand(() => DMXValues.Remove(item));
                    item.ParentCollection = DMXValues;
                }
            }
        }

        private int _startTime;
        [Range(0, int.MaxValue,
            ErrorMessage = "Start Time must be equal to or greater than 0")]
        public int StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value, true);
        }

        private int _timeSpan;
        [Range(1, int.MaxValue,
            ErrorMessage = "Time Span must be greater than 0")]
        public int TimeSpan
        {
            get => _timeSpan;
            set => SetProperty(ref _timeSpan, value, true);
        }

        private ObservableCollection<DMXValue> _dmxValues = new ObservableCollection<DMXValue>();
        public ObservableCollection<DMXValue> DMXValues
        {
            get { return _dmxValues; }
        }

        private ICommand _addDMXValue;
        public ICommand AddDMXValue
        {
            get => _addDMXValue;
            set => SetProperty(ref _addDMXValue, value);
        }

        private ICommand _deleteTimeBlock;
        public ICommand DeleteTimeBlock
        {
            get => _deleteTimeBlock;
            set => SetProperty(ref _deleteTimeBlock, value);
        }

        private ICommand _sortDMXValues;
        public ICommand SortDMXValues
        {
            get => _sortDMXValues;
            set => SetProperty(ref _sortDMXValues, value);
        }

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            #region Children

            errors.AddRange(DMXValues.SelectMany(x => x.Validate().
                Select(y => String.Format("DMX Channel {0} - {1}", x.Channel, y))));

            #endregion

            return errors;
        }
    }
}
