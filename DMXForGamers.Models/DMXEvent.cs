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
    public class DMXEvent : ObservableValidator
    {
        public DMXEvent()
        {
            TimeBlocks.CollectionChanged += TimeBlocks_CollectionChanged;

            AddTimeBlock = new RelayCommand(() =>
            {
                TimeBlocks.Add(new DMXTimeBlock());
            });

            SortTimeBlocks = new RelayCommand(() =>
            {
                TimeBlocks.Sort((x, y) => x.StartTime - y.StartTime);
            });

            ValidateAllProperties();
        }

        private void TimeBlocks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems?.Cast<DMXTimeBlock>();
            var newItems = e.NewItems?.Cast<DMXTimeBlock>();

            if (oldItems != null)
            {
                foreach (var item in oldItems)
                {
                    item.DeleteTimeBlock = null;
                }
            }

            if (newItems != null)
            {
                foreach (var item in newItems)
                {
                    item.DeleteTimeBlock = new RelayCommand(() => TimeBlocks.Remove(item));
                }
            }
        }

        public IEnumerable<DMXEvent> ParentCollection { get; set; }

        private string _eventID;
        [Required(ErrorMessage = "Event ID is Required")]
        [CustomValidation(typeof(DMXEvent), nameof(ValidateEventId))]
        public string EventID
        {
            get => _eventID;
            set
            {
                SetProperty(ref _eventID, value, true);
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
            get => _timeSpan;
            set => SetProperty(ref _timeSpan, value, true);
        }

        private int _repeatCount = 1;
        public int RepeatCount
        {
            get => _repeatCount;
            set => SetProperty(ref _repeatCount, value, true);
        }

        private string _soundFileName;
        public string SoundFileName
        {
            get => _soundFileName;
            set => SetProperty(ref _soundFileName, value, true);
        }

        private byte[] _soundData;
        public byte[] SoundData
        {
            get => _soundData;
            set => SetProperty(ref _soundData, value, true);
        }

        private DeepObservableCollection<DMXTimeBlock> _timeBlocks = new DeepObservableCollection<DMXTimeBlock>(new List<string> { nameof(DMXTimeBlock.DeleteTimeBlock) });
        public DeepObservableCollection<DMXTimeBlock> TimeBlocks
        {
            get { return _timeBlocks; }
        }

        private ICommand _addTimeBlock;
        public ICommand AddTimeBlock
        {
            get => _addTimeBlock;
            set => SetProperty(ref _addTimeBlock, value, true);
        }

        private ICommand _deleteEvent;
        public ICommand DeleteEvent
        {
            get => _deleteEvent;
            set => SetProperty(ref _deleteEvent, value, true);
        }

        private ICommand _sortTimeBlocks;
        public ICommand SortTimeBlocks
        {
            get => _sortTimeBlocks;
            set => SetProperty(ref _sortTimeBlocks, value, true);
        }

        #region Custom validation

        public static ValidationResult ValidateEventId(string name, ValidationContext context)
        {
            var instance = (DMXEvent)context.ObjectInstance;

            if (instance.ParentCollection != null)
            {
                var duplicateCount = instance.ParentCollection.Where(x => x != instance)
                    .Count(x => String.Compare(x.EventID, instance.EventID) == 0);
                if (duplicateCount > 0)
                {
                    return new ValidationResult("Event ID is duplicated in another event");
                }
            }

            return ValidationResult.Success;
        }

        #endregion

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            #region Children

            errors.AddRange(TimeBlocks.SelectMany(x => x.Validate().
                Select(y => String.Format("Time Block ({0}ms) - {1}", x.StartTime, y))));

            #endregion

            return errors;
        }
    }
}
