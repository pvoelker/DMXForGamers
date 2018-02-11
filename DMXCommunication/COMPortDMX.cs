using System;
using System.IO.Ports;
using System.Threading;

namespace DMXCommunication
{
    public class COMPortDMX : IDMXCommunication
    {
        private byte[] ZERO_BUFFER = new byte[] { 0x00 };
        private byte[] _buffer = new byte[513];
        private SerialPort _serialPort = null;
        private EventWaitHandle _done = null;
        private EventWaitHandle _doneComplete = null;
        private bool _doneStarted = false;

        static public Guid ID = new Guid("2900cfbe-a150-41d9-bf92-fbbb27fe2f22");
        public Guid Identifier { get { return ID; } }
        public string Description { get { return "COM Port (RS485)"; } }

        public string PortName { get; set; }

        public void Start()
        {
            if (String.IsNullOrWhiteSpace(PortName) == true)
            {
                throw new Exception("Port name has not been set");
            }

            _serialPort = new SerialPort(PortName, 250000, Parity.None, 8, StopBits.Two);
            _doneStarted = false;

            ClearChannelValues();

            _done = new EventWaitHandle(false, EventResetMode.ManualReset);
            _doneComplete = new EventWaitHandle(false, EventResetMode.ManualReset);

            try
            {
                Thread thread = new Thread(new ThreadStart(WriteData));
                thread.Name = "COM Port DMX Comms";
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

                    if (_serialPort != null)
                    {
                        _serialPort.Dispose();
                        _serialPort = null;
                    }
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
            while (_done.WaitOne(25) == false)
            {
                lock (_buffer)
                {
                    _serialPort.BaudRate = 96000;
                    _serialPort.Write(ZERO_BUFFER, 0, ZERO_BUFFER.Length);
                    _serialPort.BaudRate = 250000;
                    _serialPort.Write(_buffer, 0, _buffer.Length);
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

        ~COMPortDMX()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();

            if (disposing)
            {
                // Free managed resources
            }

            // Free native resources if there are any
        }

        #endregion
    }
}

