using System;
using System.IO;
using System.Xml.Serialization;

namespace DMXForGamers
{
	public static class AppSettingsFile
	{
		public static AppSettings LoadFile (string path)
		{
			using (var stream = new StreamReader (path, true)) {
				XmlSerializer serializer = new XmlSerializer (typeof(AppSettings));
				return (AppSettings)serializer.Deserialize (stream);
			}
		}

		public static void SaveFile (AppSettings data, string path)
		{
			using (var stream = new StreamWriter (path, false)) {
				XmlSerializer serializer = new XmlSerializer (typeof(AppSettings));
				serializer.Serialize (stream, data);
			}
		}

	}
}