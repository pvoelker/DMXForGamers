using System;
using System.Runtime.InteropServices;
using System.Threading;
using FTDIChip.FTD2XX;

namespace DMXCommunication
{
    public class OpenDMX : IDMXCommunication
    {
        private byte[] _buffer = new byte[513];
        private uint _handle = 0;
        private EventWaitHandle _done = null;
        private EventWaitHandle _doneComplete = null;
        private bool _doneStarted = false;
        private int _bytesWritten = 0;
        private FT_STATUS _status = FT_STATUS.FT_OK;

        public const byte BITS_8 = 8;
        public const byte STOP_BITS_2 = 2;
        public const byte PARITY_NONE = 0;
        public const UInt16 FLOW_NONE = 0;
        public const byte PURGE_RX = 1;
        public const byte PURGE_TX = 2;

        static public Guid ID = new Guid("b76abe7d-ef4a-4b05-bf41-bb4e68613ed7");
        public Guid Identifier { get { return ID; } }
        public string Description { get { return "Enttec Open DMX (FTDI Based)"; } }

        private void Init()
        {
            _status = NativeMethods.FT_ResetDevice(_handle);
            CheckStatusAndThrowException(_status);
            _status = NativeMethods.FT_SetDivisor(_handle, (char)12);  // Set baud rate
            CheckStatusAndThrowException(_status);
            _status = NativeMethods.FT_SetDataCharacteristics(_handle, BITS_8, STOP_BITS_2, PARITY_NONE);
            CheckStatusAndThrowException(_status);
            _status = NativeMethods.FT_SetFlowControl(_handle, (char)FLOW_NONE, 0, 0);
            CheckStatusAndThrowException(_status);
            _status = NativeMethods.FT_ClrRts(_handle);
            CheckStatusAndThrowException(_status);
            _status = NativeMethods.FT_Purge(_handle, PURGE_TX);
            CheckStatusAndThrowException(_status);
            _status = NativeMethods.FT_Purge(_handle, PURGE_RX);
            CheckStatusAndThrowException(_status);
        }

        public object Settings
        {
            get { return null; }
        }

        public void Start()
        {
            _handle = 0;
            _status = NativeMethods.FT_Open(0, ref _handle);
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

                    if (_handle != 0)
                    {
                        NativeMethods.FT_Close(_handle);
                        _handle = 0;
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
                Init();

                _status = NativeMethods.FT_SetBreakOn(_handle);
                _status = NativeMethods.FT_SetBreakOff(_handle);
                lock (_buffer)
                {
                    _bytesWritten = Write(_handle, _buffer, _buffer.Length);
                }
            }

            _doneComplete.Set();
        }

        private int Write(uint handle, byte[] data, int length)
        {
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(length);
                uint bytesWritten = 0;
                try
                {
                    Marshal.Copy(data, 0, ptr, length);
                    _status = NativeMethods.FT_Write(handle, ptr, (uint)length, ref bytesWritten);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                    ptr = IntPtr.Zero;
                }
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