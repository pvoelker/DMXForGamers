using DMXCommunication.Converters;
using System;
using System.ComponentModel;
using System.Net;
using System.Xml.Serialization;

namespace DMXCommunication.Settings
{
    /// <summary>
    /// Represents the configuration settings for sending DMX data using the Art-Net protocol
    /// </summary>
    [Serializable]
    public class ArtNetDMXSettings : BaseSettings
    {
        [Category("Configuration")]
        [DisplayName("Node IP Address")]
        [Description("This property specifies the IP address to send the DMX data (ArtDmx) packets to.")]
        [TypeConverter(typeof(IPAddressConverter))]
        [XmlIgnore]
        public IPAddress NodeIPAddress { get; set; } = IPAddress.Parse("127.0.0.1");
        [XmlElement("NodeIPAddress")]
        [Browsable(false)]
        public string MasterIPForXML
        {
            get
            {
                return NodeIPAddress.ToString();
            }
            set
            {
                NodeIPAddress = String.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
            }
        }

        [Category("Configuration")]
        [DisplayName("Universe")]
        [Description("This property defines the 'universe' to be used.")]
        public ushort Universe { get; set; } = 0;

        [Category("Configuration")]
        [DisplayName("Port")]
        [Description("This property defines the UDP port number to be used (default 6454).")]
        [TypeConverter(typeof(IPAddressConverter))]
        public ushort Port { get; set; } = 6454;
    }
}
