using System;
using System.IO;
using System.Xml.Serialization;

namespace DMXEngine
{
	public static class EventDefinitionsFile
	{
		public static EventDefinitions LoadFile (string path)
		{
			using (var stream = new StreamReader (path, true)) {
				if(stream.BaseStream.CanSeek == true)
					stream.BaseStream.Seek (0, SeekOrigin.Begin);
				XmlSerializer serializer = new XmlSerializer (typeof(EventDefinitions));
				return (EventDefinitions)serializer.Deserialize (stream);
			}
		}

		public static void SaveFiles (EventDefinitions data, string path)
		{
			using (var stream = new StreamWriter (path, false)) {
				XmlSerializer serializer = new XmlSerializer (typeof(EventDefinitions));
				serializer.Serialize (stream, data);
			}
		}
	}
}

