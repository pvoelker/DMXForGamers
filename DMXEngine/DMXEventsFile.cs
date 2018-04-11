using System;
using System.IO;
using System.Xml.Serialization;

namespace DMXEngine
{
    public static class DMXEventsFile
    {
        public static DMX LoadFile(string path)
        {
            using (var stream = new StreamReader(path, true))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DMX));
                return (DMX)serializer.Deserialize(stream);
            }
        }

        public static void SaveFile(DMX data, string path)
        {
            using (var stream = new StreamWriter(path, false))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DMX));
                serializer.Serialize(stream, data);
            }
        }
    }
}

