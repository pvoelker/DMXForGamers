using System.Xml.Serialization;
using System.Collections.Generic;
using System;

namespace DMXEngine
{
    [XmlRoot]
    public class DMX
    {
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private List<DMXValue> _baseDMXValues = new List<DMXValue>();
        public List<DMXValue> BaseDMXValues
        {
            get { return _baseDMXValues; }
        }

        private List<Event> _events = new List<Event>();
        public List<Event> Events
        {
            get { return _events; }
        }
    }

    public class Event
    {
        public Event()
        {
        }
        public Event(string id, int timeSpan)
        {
            ID = id;
            TimeSpan = timeSpan;
        }
		public Event(string id, int timeSpan, int repeatCount)
		{
			ID = id;
			TimeSpan = timeSpan;
			RepeatCount = repeatCount;
		}

		private string _id;
        [XmlAttribute]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _timeSpan;
        [XmlAttribute]
        public int TimeSpan
        {
            get { return _timeSpan; }
            set { _timeSpan = value; }
        }

		private int _repeatCount = 1;
		[XmlAttribute]
		public int RepeatCount
		{
			get { return _repeatCount; }
			set { _repeatCount = value; }
		}

		private List<TimeBlock> _timeBlocks = new List<TimeBlock>();
        public List<TimeBlock> TimeBlocks
        {
            get { return _timeBlocks; }
        }
    }

    public class TimeBlock
    {
        public TimeBlock()
        {
        }
		public TimeBlock(int startTime, int timeSpan)
        {
            StartTime = startTime;
			TimeSpan = timeSpan;
        }

        private int _startTime;
        [XmlAttribute]
        public int StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

		private int _timeSpan;
		[XmlAttribute]
		public int TimeSpan
		{
			get { return _timeSpan; }
			set { _timeSpan = value; }
		}

		private List<DMXValue> _dmxValues = new List<DMXValue>();
        public List<DMXValue> DMXValues
        {
            get { return _dmxValues; }
        }
    }

    public class DMXValue
    {
        public DMXValue()
        {
        }
        public DMXValue(ushort channel, byte value)
        {
            Channel = channel;
            Value = value;
        }
		public DMXValue(ushort channel, byte value, short delta)
		{
			Channel = channel;
			Value = value;
			Delta = delta;
		}

		private ushort _channel;
        [XmlAttribute]
		public ushort Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        private byte _value;
        [XmlAttribute]
        public byte Value
        {
            get { return _value; }
            set { _value = value; }
        }

		private short _delta;
		[XmlAttribute]
		public short Delta
		{
			get { return _delta; }
			set { _delta = value; }
		}
    }
}
