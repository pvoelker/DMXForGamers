using System;
using System.Xml;
using System.Xml.Serialization;

namespace DMXEngine
{
    public static class DMXEventsFile
    {
        private static readonly XmlSerializer _serializer = null;

        private static readonly XmlReaderSettings _readerSettings = new()
        {
            ConformanceLevel = ConformanceLevel.Document
        };

        private static readonly XmlWriterSettings _writeSettings = new()
        {
            Encoding = System.Text.Encoding.UTF8,
            NewLineChars = Environment.NewLine,
            Indent = true,
            ConformanceLevel = ConformanceLevel.Document
        };

        static DMXEventsFile()
        {
            _serializer = new XmlSerializer(typeof(DMX));
        }

        public static DMX LoadFile(string path)
        {
            using var xmlReader = XmlReader.Create(path, _readerSettings);

            return (DMX)_serializer.Deserialize(xmlReader);
        }

        public static void SaveFile(DMX data, string path)
        {
            using var xmlWriter = XmlWriter.Create(path, _writeSettings);

            _serializer.Serialize(xmlWriter, data);
        }
    }
}

