using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class EventDefinition : NotifyPropertyChangedWithErrorInfoBase
    {
        public EventDefinition()
        {
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; AnnouncePropertyChanged(); }
        }

        private string _eventID;
        public string EventID
        {
            get { return _eventID; }
            set { _eventID = value; AnnouncePropertyChanged(); }
        }

        private bool _state;
        public bool State
        {
            get { return _state; }
            set { _state = value; AnnouncePropertyChanged(); }
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
                _useRegEx = value;
                AnnouncePropertyChanged();
                OnPropertyChanged(nameof(Pattern)); // For error validation
            }
        }

        private string _pattern;
        public string Pattern
        {
            get { return _pattern; }
            set { _pattern = value; AnnouncePropertyChanged(); }
        }

        private bool _continuous;
        public bool Continuous
        {
            get { return _continuous; }
            set { _continuous = value; AnnouncePropertyChanged(); }
        }

        private ICommand _eventOn;
        public ICommand EventOn
        {
            get { return _eventOn; }
            set { _eventOn = value; AnnouncePropertyChanged(); }
        }

        private ICommand _eventOff;
        public ICommand EventOff
        {
            get { return _eventOff; }
            set { _eventOff = value; AnnouncePropertyChanged(); }
        }

        private ICommand _deleteEvent;
        public ICommand DeleteEvent
        {
            get { return _deleteEvent; }
            set { _deleteEvent = value; AnnouncePropertyChanged(); }
        }

        #region IErrorInfo

        public override string this[string columnName]
        {
            get
            {
                var errorStr = new StringBuilder();

                if ((columnName == nameof(EventID)) || (columnName == null))
                {
                    if (String.IsNullOrWhiteSpace(EventID) == true)
                    {
                        errorStr.AppendLine("Event ID is required");
                    }
                }
                if ((columnName == nameof(Pattern)) || (columnName == null))
                {
                    if (UseRegEx == true)
                    {
                        if (String.IsNullOrWhiteSpace(Pattern) == true)
                        {
                            errorStr.AppendLine("Matching Pattern is required or disable Regular Expression option");
                        }
                    }
                }

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion
    }
}
