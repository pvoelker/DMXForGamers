using System;
using System.Threading;
using FTD2XX_NET;
using static FTD2XX_NET.FTDI;

namespace DMXCommunication
{
    public class OpenDMX : IDMXCommunication
    {
        private readonly byte[] _buffer = new byte[513];
        private int _bytesWritten = 0;
        private EventWaitHandle _done = null;
        private EventWaitHandle _doneComplete = null;
        private bool _doneStarted = false;
        private FT_STATUS _status = FT_STATUS.FT_OK;

        private readonly FTDI _ftdi = new FTDI();

        static public Guid ID = new Guid("b76abe7d-ef4a-4b05-bf41-bb4e68613ed7");
        public Guid Identifier { get { return ID; } }
        public string Description { get { return "Enttec Open DMX (FTDI Based)"; } }

        private void Init()
        {
            _status = _ftdi.ResetDevice();
            CheckStatusAndThrowException(_status);
            _status = _ftdi.SetBaudRate(250000);
            CheckStatusAndThrowException(_status);
            _status = _ftdi.SetDataCharacteristics(FT_DATA_BITS.FT_BITS_8, FT_STOP_BITS.FT_STOP_BITS_2, FT_PARITY.FT_PARITY_NONE);
            CheckStatusAndThrowException(_status);
            _status = _ftdi.SetFlowControl(FT_FLOW_CONTROL.FT_FLOW_NONE, 0, 0);
            CheckStatusAndThrowException(_status);
            _status = _ftdi.SetRTS(false);
            CheckStatusAndThrowException(_status);
            _status = _ftdi.Purge(FT_PURGE.FT_PURGE_TX);
            CheckStatusAndThrowException(_status);
            _status = _ftdi.Purge(FT_PURGE.FT_PURGE_RX);
            CheckStatusAndThrowException(_status);
        }

        public object Settings
        {
            get { return null; }
        }

        public void Start()
        {
            _status = _ftdi.OpenByIndex(0);
            CheckStatusAndThrowException(_status);
            _doneStarted = false;

            ClearChannelValues();

            _done = new EventWaitHandle(false, EventResetMode.ManualReset);
            _doneComplete = new EventWaitHandle(false, EventResetMode.ManualReset);

            try
            {
                Thread thread = new Thread(new ThreadStart(WriteData));
                thread.Name = "OpenDMX Comms";
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

                    _ftdi.Close();
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
                throw new ArgumentOutOfRangeException(nameof(channel), channel, "Valid range is 1 through 512");
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
                Init();

                _status = _ftdi.SetBreak(true);
                _status = _ftdi.SetBreak(false);

                lock (_buffer)
                {
                    _bytesWritten = Write(_buffer, _buffer.Length);
                }
            }

            _doneComplete.Set();
        }

        private int Write(byte[] data, int length)
        {
            try
            {
                uint bytesWritten = 0;
                
                _status = _ftdi.Write(data, length, ref bytesWritten);
                
                return (int)bytesWritten;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return 0;
            }
        }

        private void CheckStatusAndThrowException(FT_STATUS status)
        {
            if (status != FT_STATUS.FT_OK)
            {
                string msg = String.Format("Call to 'FTD2XX.dll' failed. Error code: {0} ({1})",
                    (int)status, status.ToString());
                throw new Exception(msg);
            }
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~OpenDMX()
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