using System;
using System.IO;
using System.Xml.Serialization;

namespace DMXEngine
{
	public static class DMXEventsFile
	{
		public static DMX LoadFile (string path)
		{
			using (var stream = new StreamReader (path, true)) {
				XmlSerializer serializer = new XmlSerializer (typeof(DMX));
				return (DMX)serializer.Deserialize (stream);
			}
		}

		public static void SaveFiles (DMX data, string path)
		{
			using (var stream = new StreamWriter (path, false)) {
				XmlSerializer serializer = new XmlSerializer (typeof(DMX));
				serializer.Serialize (stream, data);
			}
		}

//		public static bool ValidateData(DMX data)
//		{
//			foreach (var dmxEvent in data.Events) {
//				foreach (var timeBlock in dmxEvent.TimeBlocks) {
//					foreach (var temp in timeBlock.DMXValues) {
//					}
//				}
//			}
//		}
//
//		public static void CleanData (DMX data)
//		{
//			// Make sure time blocks are sorted in ascending order
//			foreach (var dmxEvent in data.Events) {
//				dmxEvent.TimeBlocks.Sort ((x, y) => x.StartTime > y.StartTime);
//			}
//		}
	}
}

