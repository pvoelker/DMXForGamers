using NAudio.Wave;
using System;

namespace DMXEngine.Audio
{
    public class AutoDisposeFileReader : ISampleProvider
    {
        private readonly AudioFileReader _reader;
        private bool _isDisposed;

        public AutoDisposeFileReader(AudioFileReader reader)
        {
            _reader = reader;
            WaveFormat = reader.WaveFormat;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (_isDisposed)
            {
                return 0;
            }

            int read = _reader.Read(buffer, offset, count);

            if (read == 0)
            {
                _reader.Dispose();
                _isDisposed = true;
            }

            return read;
        }

        public WaveFormat WaveFormat { get; private set; }
    }
}
