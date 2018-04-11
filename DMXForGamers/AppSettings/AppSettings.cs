using System;
using System.Xml;
using System.Xml.Serialization;

namespace DMXForGamers
{
	[XmlRoot]
	public class AppSettings
	{
		public Guid PortAdapterGuid { get; set; }
		public int TextMonitorOption { get; set; }
		public string EXEFilePath { get; set; }
		public string EXEArguments { get; set; }
		public string MonitorFilePath { get; set; }
		public string DMXFilePath { get; set; }
		public string EventDefinitionsFilePath { get; set; }

        public bool EnableRemoteControl { get; set; }
        public ushort RemoteControlPort { get; set; }
	}
}

