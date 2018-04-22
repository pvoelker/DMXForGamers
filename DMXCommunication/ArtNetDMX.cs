using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace DMXCommunication
{
    public class ArtNetMessage
    {
        public ArtNetMessage(int universe, List<byte> dmxValues)
        {
            Universe = universe;
            DMXValues = dmxValues;
        }

        public int Universe { get; set; }

        public List<byte> DMXValues { get; set; }

        public byte[] ToByteArray()
        {
            var bytes = new byte[530];

            bytes[0] = (byte)'A';
            bytes[1] = (byte)'r';
            bytes[2] = (byte)'t';
            bytes[3] = (byte)'-';
            bytes[4] = (byte)'N';
            bytes[5] = (byte)'e';
            bytes[6] = (byte)'t';
            bytes[7] = 0;

            // Opcode
            bytes[8] = 0x50;
            bytes[9] = 0x00;

            // Protocl Version
            bytes[10] = 0;
            bytes[11] = 14;

            // Sequence (not used)
            bytes[12] = 0;

            // Physical (not used???)
            bytes[13] = 0;

            // Universe
            var universeBytes = IntToByteArray(Universe);
            bytes[14] = universeBytes[0];
            bytes[15] = universeBytes[1];

            // Length (512)
            var lengthBytes = IntToByteArray(512);
            bytes[16] = lengthBytes[0];
            bytes[17] = lengthBytes[1];

            for (int i = 0; i < 512; i++)
            {
                bytes[18 + i] = DMXValues[i];
            }

            return bytes;
        }

        private byte[] IntToByteArray(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }
    }

    // --------------------------------------------------------------------------------------------

    public class IPAddressConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
                return IPAddress.Parse((string)value);
            return base.ConvertFrom(context, culture, value);
        }
    }

    // --------------------------------------------------------------------------------------------

    [Serializable]
    public class ArtNetDMXSettings
    {
        [Category("Configuration")]
        [DisplayName("Universe")]
        [Description("This property defines the 'universe' to be used.")]
        //[DefaultValue((int)0)]
        public int Universe { get; set; } = 0;

        [Category("Configuration")]
        [DisplayName("Node IP Address")]
        [Description("This property defines the 'universe' to be used.")]
        //[DefaultValue(2130706433/*127.0.0.1*/)]
        [TypeConverter(typeof(IPAddressConverter))]
        public IPAddress NodeIPAddress { get; set; } = IPAddress.Parse("127.0.0.1");

        [Category("Configuration")]
        [DisplayName("Port")]
        [Description("This property defines the UDP port number to be used (default 6454).")]
        //[DefaultValue(6454)]
        [TypeConverter(typeof(IPAddressConverter))]
        public ushort Port { get; set; } = 6454;
    }

    public class ArtNetDMX : IDMXCommunication
    {
        private IPEndPoint _endPoint;
        private Socket _socket;
        private byte[] _buffer = new byte[513];
        private EventWaitHandle _done = null;
        private EventWaitHandle _doneComplete = null;
        private bool _doneStarted = false;
        private int _bytesWritten = 0;

        static public Guid ID = new Guid("24597d14-f33b-4385-a909-7a232d0327d4");
        public Guid Identifier { get { return ID; } }
        public string Description { get { return "Art-Net (DMX over IP)"; } }

        #region Settings

        private static ArtNetDMXSettings _settings = new ArtNetDMXSettings();

        public ArtNetDMXSettings Settings
        {
            get { return _settings; }
        }

        object IDMXCommunication.Settings
        {
            get { return Settings; }
        }

        #endregion

        public void Start()
        {
            //var permission = new SocketPermission(NetworkAccess.Accept, TransportType.Udp, "", Settings.Port);

            _endPoint = new IPEndPoint(Settings.NodeIPAddress, Settings.Port);
            _socket = new Socket(_endPoint.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            _doneStarted = false;

            ClearChannelValues();

            _done = new EventWaitHandle(false, EventResetMode.ManualReset);
            _doneComplete = new EventWaitHandle(false, EventResetMode.ManualReset);

            try
            {
                Thread thread = new Thread(new ThreadStart(WriteData));
                thread.Name = "Art-Net DMX Comms";
                thread.Start();
            }
            catch
            {
                _done.Dispose();
                _done = null;
                _doneComplete.Dispose();
                _doneComplete = null;

                throw;
            }
        }

        public void Stop()
        {
            if (_done != null)
            {
                try
                {
                    _doneStarted = true;

                    ClearChannelValuesInternal();

                    // Wait for cleared channels to be written out
                    Thread.Sleep(50);

                    _done.Set();
                    _doneComplete.WaitOne();

                    _socket.Dispose();
                    _socket = null;
                    _endPoint = null;
                }
                finally
                {
                    _done.Dispose();
                    _done = null;
                    _doneComplete.Dispose();
                    _doneComplete = null;
                }
            }
        }

        public void ClearChannelValues()
        {
            if (_doneStarted == false)
            {
                ClearChannelValuesInternal();
            }
        }

        private void ClearChannelValuesInternal()
        {
            lock (_buffer)
            {
                for (ushort i = 1; i < _buffer.Length; i++)
                {
                    _buffer[i] = 0;
                }
            }
        }

        public void SetChannelValue(ushort channel, byte value)
        {
            if ((channel < 1) || (channel > 512))
            {
                throw new ArgumentOutOfRangeException("channel", channel, "Valid range is 1 through 512");
            }

            if (_doneStarted == false)
            {
                lock (_buffer)
                {
                    _buffer[channel] = value;
                }
            }
        }

        private void WriteData()
        {
            // Art-Net recommends refreshing at 44 time a second (http://art-net.org.uk/wordpress/structure/streaming-packets/)
            const int ARTNET_REFRESH = 1000 / 44;
            while (_done.WaitOne(ARTNET_REFRESH) == false)
            {
                lock (_buffer)
                {
                    var message = new ArtNetMessage(Settings.Universe, new List<byte>(_buffer));
                    var buffer = message.ToByteArray();
                    _bytesWritten = _socket.SendTo(buffer, _endPoint);
                }
            }

            _doneComplete.Set();
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ArtNetDMX()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();

            if (disposing)
            {
                // Free managed 
            }

            // Free native resources if there are any
        }

        #endregion
    }
}