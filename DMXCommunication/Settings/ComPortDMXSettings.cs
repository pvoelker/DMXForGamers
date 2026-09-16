using System;
using System.ComponentModel;

namespace DMXCommunication.Settings
{
    /// <summary>
    /// Represents the configuration settings for a DMX device that communicates over a RS485 port
    /// </summary>
    [Serializable]
    public class ComPortDMXSettings : BaseSettings
    {
        [Category("Configuration")]
        [DisplayName("Port Name")]
        [Description("This property defines the RS485 port name to be used.")]
        public string PortName { get; set; } = "COM1";
    }
}
