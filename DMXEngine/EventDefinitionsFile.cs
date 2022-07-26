using System;
using System.Xml;
using System.Xml.Serialization;

namespace DMXEngine
{
    public static class EventDefinitionsFile
    {
        private static XmlSerializer _serializer = null;

        static EventDefinitionsFile()
        {
            _serializer = new XmlSerializer(typeof(EventDefinitions));
        }

        public static EventDefinitions LoadFile(string path)
        {
            var settings = new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document
            };

            using (var xmlReader = XmlReader.Create(path, settings))
            {
                return (EventDefinitions)_serializer.Deserialize(xmlReader);
            }
        }

        public static void SaveFile(EventDefinitions data, string path)
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

