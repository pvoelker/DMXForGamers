using NAudio.Wave;
using System;

namespace DMXEngine.Audio
{
    public class InMemorySound
    {
        public byte[] AudioData
        {
            get;
            private set;
        }
        
        public WaveFormat WaveFormat
        {
            get;
            private set;
        }

        public InMemorySound(WaveFormat format, byte[] audioData)
        {
            WaveFormat = format;
            AudioData = audioData
        }
    }
}
