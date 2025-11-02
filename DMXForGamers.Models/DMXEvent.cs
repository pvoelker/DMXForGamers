using DMXForGamers.Models.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
                int newStartTime = 0;
                if (TimeBlocks.Count > 0)
                {
                    newStartTime = TimeBlocks.Max(x2 => x2.StartTime + x2.TimeSpan);
                }

                TimeBlocks.Add(new DMXTimeBlock
                {
                    StartTime = newStartTime
                });
            });

            AddTimeBlockBeforeSelected = new RelayCommand(() =>
            {
                var index = TimeBlocks.IndexOf(SelectedTimeBlock);
                TimeBlocks.Insert(index, new DMXTimeBlock()
                {
                    StartTime = SelectedTimeBlock.StartTime
                });
            }, () => SelectedTimeBlock != null);

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

        public WeakReference<IReadOnlyCollection<DMXEvent>> ParentCollection { get; set; }

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

        private DeepObservableCollection<DMXTimeBlock> _timeBlocks = new DeepObservableCollection<DMXTimeBlock>(new HashSet<string> { nameof(DMXTimeBlock.DeleteTimeBlock) });
        public DeepObservableCollection<DMXTimeBlock> TimeBlocks
        {
            get { return _timeBlocks; }
        }

        private DMXTimeBlock _selectedTimeBlock;
        public DMXTimeBlock SelectedTimeBlock
        {
            get => _selectedTimeBlock;
            set
            {
                SetProperty(ref _selectedTimeBlock, value, false);
                AddTimeBlockBeforeSelected.NotifyCanExecuteChanged();
            }
        }

        private ICommand _addTimeBlock;
        public ICommand AddTimeBlock
        {
            get => _addTimeBlock;
            set => SetProperty(ref _addTimeBlock, value, true);
        }

        private RelayCommand _addTimeBlockBeforeSelected;
        public RelayCommand AddTimeBlockBeforeSelected
        {
            get => _addTimeBlockBeforeSelected;
            set => SetProperty(ref _addTimeBlockBeforeSelected, value, true);
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
                if (instance.ParentCollection.TryGetTarget(out var parentCollection))
                {
                    var duplicateCount = parentCollection.Where(x => x != instance)
                        .Count(x => String.Compare(x.EventID, instance.EventID) == 0);
                    if (duplicateCount > 0)
                    {
                        return new ValidationResult("Event ID is duplicated in another event");
                    }
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
