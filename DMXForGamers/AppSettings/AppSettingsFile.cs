using DMXEngine;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace DMXForGamers
{
    public static class AppSettingsFile
    {
        private static XmlSerializer _serializer = null;

        static AppSettingsFile()
        {
            _serializer = new XmlSerializer(typeof(AppSettings));
        }

        public static AppSettings LoadFile(string path)
        {
            var settings = new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document
            };

            using (var xmlReader = XmlReader.Create(path, settings))
            {
                return (AppSettings)_serializer.Deserialize(xmlReader);
            }
        }

        public static void SaveFile(AppSettings data, string path)
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