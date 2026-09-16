using System;
using System.Xml.Serialization;

namespace DMXCommunication.Settings
{
    /// <summary>
    /// Serves as the base class for settings objects that provide configuration data for DMX communication implementations
    /// </summary>
    /// <remarks>XmlIncludes will need to be added as additional settings classes use this base class</remarks>
    [Serializable]
    [XmlInclude(typeof(ArtNetDMXSettings))]
    [XmlInclude(typeof(ComPortDMXSettings))]
    public class BaseSettings
    {
    }
}
