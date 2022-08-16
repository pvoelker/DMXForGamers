using NAudio.Wave;
using System;

namespace DMXEngine.Audio
{
    public class InMemorySoundSampleProvider : ISampleProvider
    {
        private readonly InMemorySound _inMemorySound;
        private long _position;

        public InMemorySoundSampleProvider(InMemorySound inMemorySound)
        {
            this._inMemorySound = inMemorySound;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = _inMemorySound.AudioData.Length - _position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(_inMemorySound.AudioData, _position, buffer, offset, samplesToCopy);
            _position += samplesToCopy;
            return (int)samplesToCopy;
        }

        public WaveFormat WaveFormat
        {
            get { return _inMemorySound.WaveFormat; }
        }
    }
}
