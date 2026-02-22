using System;

namespace DMXCommunication.Models
{
    /// <summary>
    /// Represents a DMX port adapter, including its description, unique identifier, type, and configuration settings.
    /// </summary>
    public class DMXPortAdapter
    {
        public DMXPortAdapter(string description, Guid id, Type type, object settings)
        {
            Description = description;
            ID = id;
            Type = type;
            Settings = settings;
        }

        public string Description { get; private set; }
        public Guid ID { get; private set; }
        public Type Type { get; private set; }
        public object Settings { get; private set; }
    }
}
