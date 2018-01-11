using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class EventDefinition : NotifyPropertyChangedBase
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
    }
}
