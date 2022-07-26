using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DMXEngine
{
    public static class DMXEventsFile
    {
        private static XmlSerializer _serializer = null;

        static DMXEventsFile()
        {
            _serializer = new XmlSerializer(typeof(DMX));
        }

        public static DMX LoadFile(string path)
        {
            var settings = new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document
            };

            using (var xmlReader = XmlReader.Create(path, settings))
            {
                return (DMX)_serializer.Deserialize(xmlReader);
            }
        }

        public static void SaveFile(DMX data, string path)
        {
            var settings = new XmlWriterSettings()
            {
                Encoding = System.Text.Encoding.UTF8,
                NewLineChars = Environment.NewLine,
                Indent = true,
                ConformanceLevel = ConformanceLevel.Document
            };

            using (var xmlWriter = XmlWriter.Create(path, settings))
            {
                _serializer.Serialize(xmlWriter, data);
            }
        }
    }
}

