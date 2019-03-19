using System.Xml.Serialization;
using System.Collections.Generic;
using System;

namespace DMXEngine
{
	[XmlRoot]
	public class EventDefinitions
	{
		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        private List<EventDefinition> _Events = new List<EventDefinition>();
		public List<EventDefinition> Events
		{
			get { return _Events; }
            set { _Events = value; }
		}
	}

	public class EventDefinition
	{
		public EventDefinition()
		{
		}
		public EventDefinition(string description, string eventId, string pattern)
		{
			Description = description;
			EventID = eventId;
			Pattern = pattern;
			UseRegEx = false;
		}
		public EventDefinition(string description, string eventId, string pattern, bool useRegEx)
		{
			Description = description;
			EventID = eventId;
			Pattern = pattern;
			UseRegEx = useRegEx;
		}

		private string _description;
		[XmlAttribute]
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		private bool _useRegEx;
		[XmlAttribute]
		public bool UseRegEx
		{
			get { return _useRegEx; }
			set { _useRegEx = value; }
		}

		private string _eventID;
		public string EventID
		{
			get { return _eventID; }
			set { _eventID = value; }
		}		

		private string _pattern;
		public string Pattern
		{
			get { return _pattern; }
			set { _pattern = value; }
		}		

		private bool _continuous;
		public bool Continuous
		{
			get { return _continuous; }
			set { _continuous = value; }
		}

        private string _soundFileName;
        public string SoundFileName
        {
            get { return _soundFileName; }
            set { _soundFileName = value; }
        }

        private byte[] _soundData;
        public byte[] SoundData
        {
            get { return _soundData; }
            set { _soundData = value; }
        }
    }
}

