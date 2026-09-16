using DMXCommunication.Models;
using DMXCommunication.Settings;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DMXCommunication
{
    /// <summary>
    /// Provides Art-Net DMX (Digital Multiplex) communication over IP networks, enabling the transmission and control
    /// of DMX lighting data using the Art-Net protocol.    
    /// </summary>
    /// <remarks>ArtNetDMX implements the IDMXCommunication interface to facilitate DMX data transmission over
    /// UDP using the Art-Net protocol, commonly used in lighting control systems. The class manages network
    /// communication, channel value updates, and resource cleanup. Thread safety is maintained for channel value
    /// updates. Instances should be started with Start() before sending DMX data and stopped with Stop() when
    /// communication is no longer needed. Dispose() should be called to release resources when finished. The class is
    /// not intended for use across multiple universes simultaneously; use separate instances for each universe if
    /// required.</remarks>
    public class ArtNetDMX : IDMXCommunication
    {
        private IPEndPoint _endPoint;
        private Socket _socket;
        private readonly byte[] _buffer = new byte[512];
        private EventWaitHandle _done = null;
        private EventWaitHandle _doneComplete = null;
        private bool _doneStarted = false;
        private int _bytesWritten = 0;
        volatile bool _isActive = false;

        static public Guid ID = new Guid("24597d14-f33b-4385-a909-7a232d0327d4");
        public Guid Identifier { get { return ID; } }
        public string Description { get { return "Art-Net (DMX over IP)"; } }

        #region Settings

        private static readonly ArtNetDMXSettings _settings = new ArtNetDMXSettings();

        public ArtNetDMXSettings Settings
        {
            get { return _settings; }
        }

        object IDMXCommunication.Settings
        {
            get { return Settings; }
            set
            {
                if (value is ArtNetDMXSettings settings)
                {
                    Settings.NodeIPAddress = settings.NodeIPAddress;
                    Settings.Universe = settings.Universe;
                    Settings.Port = settings.Port;
                }
                else
                {
                    throw new ArgumentException("Invalid settings object", nameof(value));
                }
            }
        }

        #endregion

        public void Start()
        {
            _endPoint = new IPEndPoint(Settings.NodeIPAddress, Settings.Port);
            _socket = new Socket(_endPoint.Address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            _doneStarted = false;

            ClearChannelValues();

            _done = new EventWaitHandle(false, EventResetMode.ManualReset);
            _doneComplete = new EventWaitHandle(false, EventResetMode.ManualReset);

            try
            {
                Thread thread = new Thread(new ThreadStart(WriteData))
                {
                    Name = "Art-Net DMX Comms"
                };
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
                for (ushort i = 0; i < _buffer.Length; i++)
                {
                    _buffer[i] = 0;
                }
                _isActive = true;
            }
        }

        public void SetChannelValue(ushort channel, byte value)
        {
            if ((channel < 1) || (channel > 512))
            {
                throw new ArgumentOutOfRangeException(nameof(channel), channel, "Valid range is 1 through 512");
            }

            if (_doneStarted == false)
            {
                lock (_buffer)
                {
                    if (_buffer[channel - 1] != value)
                    {
                        _buffer[channel - 1] = value;
                        _isActive = true;
                    }
                }
            }
        }

        private void WriteData()
        {
            // Art-Net recommends refreshing at 44 times a second when active and every 4 seconds when idle (http://art-net.org.uk/wordpress/structure/streaming-packets/)
            const int ARTNET_REFRESH = 1000 / 44;
            const int ARTNET_REFRESH_IDLE = 3900; // Just under 4 seconds

            var lastUpdate = DateTime.MinValue;

            while (_done.WaitOne(ARTNET_REFRESH) == false)
            {
                if ((_isActive == true) || ((DateTime.Now - lastUpdate).TotalMilliseconds >= ARTNET_REFRESH_IDLE))
                {
                    _isActive = false;

                    lock (_buffer)
                    {
                        var message = new ArtNetMessage(Settings.Universe, _buffer);
                        var buffer = message.ToByteArray();
                        _bytesWritten = _socket.SendTo(buffer, _endPoint);
                    }

                    lastUpdate = DateTime.Now;
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