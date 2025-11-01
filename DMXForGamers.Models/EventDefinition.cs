using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class EventDefinition : ObservableValidator
    {
        public EventDefinition()
        {
            ValidateAllProperties();
        }

        public WeakReference<IReadOnlyCollection<EventDefinition>> ParentCollection { get; set; }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value, true); }
        }

        private string _eventID;
        [Required(ErrorMessage = "Event ID is Required")]
        [CustomValidation(typeof(EventDefinition), nameof(ValidateEventId))]
        public string EventID
        {
            get { return _eventID; }
            set
            {
                SetProperty(ref _eventID, value, true);
                OnPropertyChanged(nameof(FormattedEventID));
                OnPropertyChanged(nameof(EventIDNoSpaces));
            }
        }
        public string FormattedEventID
        {
            get { return (String.IsNullOrWhiteSpace(EventID) == true) ? "[No ID]" : EventID; }
        }

        private bool _state;
        public bool State
        {
            get { return _state; }
            set { SetProperty(ref _state, value, true); }
        }

        private TimeSpan? _executionTime;
        public TimeSpan? ExecutionTime
        {
            get { return _executionTime; }
            set
            {
                SetProperty(ref _executionTime, value, true);
                OnPropertyChanged(nameof(ExecutionTimeMs));
            }
        }

        public long? ExecutionTimeMs
        {
            get { return (long?)_executionTime?.TotalMilliseconds; }
        }

        public string EventIDNoSpaces
        {
            get { return _eventID.Replace(" ", String.Empty);  }
        }

        private bool _useRegEx;
        public bool UseRegEx
        {
            get { return _useRegEx; }
            set
            {
                SetProperty(ref _useRegEx, value, true);
                ValidateProperty(_pattern, nameof(Pattern));
                //OnPropertyChanged(nameof(Pattern)); // For error validation
            }
        }

        private string _pattern;
        [CustomValidation(typeof(EventDefinition), nameof(ValidateRegexPattern))]
        public string Pattern
        {
            get { return _pattern; }
            set { SetProperty(ref _pattern, value, true); }
        }

        private bool _continuous;
        public bool Continuous
        {
            get { return _continuous; }
            set { SetProperty(ref _continuous, value, true); }
        }

        private ICommand _eventOn;
        public ICommand EventOn
        {
            get { return _eventOn; }
            set { SetProperty(ref _eventOn, value, true); }
        }

        private ICommand _eventOff;
        public ICommand EventOff
        {
            get { return _eventOff; }
            set { SetProperty(ref _eventOff, value, true); }
        }

        private ICommand _deleteEvent;
        public ICommand DeleteEvent
        {
            get { return _deleteEvent; }
            set { SetProperty(ref _deleteEvent, value, true); }
        }

        #region Custom validation

        public static ValidationResult ValidateEventId(string name, ValidationContext context)
        {
            var instance = (EventDefinition)context.ObjectInstance;

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

        public static ValidationResult ValidateRegexPattern(string name, ValidationContext context)
        {
            var instance = (EventDefinition)context.ObjectInstance;

            if (instance.UseRegEx == true)
            {
                if (String.IsNullOrWhiteSpace(instance.Pattern) == true)
                {
                    return new ValidationResult("Matching Pattern is required or disable Regular Expression option");
                }
            }

            return ValidationResult.Success;
        }

        #endregion

        public IEnumerable<string> Validate()
        {
            var errors = new List<string>();

            errors.AddRange(GetErrors().Select(x => x.ErrorMessage));

            return errors;
        }
    }
}
