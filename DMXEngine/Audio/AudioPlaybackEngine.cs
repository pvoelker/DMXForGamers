using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.IO;

namespace DMXEngine.Audio
{
    public class AudioPlaybackEngine : IDisposable
    {
        private readonly IWavePlayer _outputDevice;
        private readonly MixingSampleProvider _mixer;

        private AudioPlaybackEngine(int sampleRate, int channelCount)
        {
            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            _mixer.ReadFully = true;

            _outputDevice = new WaveOutEvent();
            _outputDevice.Init(_mixer);
            _outputDevice.Play();
        }

        public ISampleProvider PlaySound(WaveFormat format, byte[] audioData)
        {
            //https://markheath.net/post/fire-and-forget-audio-playback-with
            //  https://stackoverflow.com/questions/37540288/how-to-convert-a-byte-array-into-wavestream-in-naudio

            //WdlResamplingSampleProvider 

            /*
            var ms = new MemoryStream(soundArray.ToArray());
            IWaveProvider provider = new RawSourceWaveStream(ms, new WaveFormat());
            var waveOut = new NAudio.Wave.WaveOut();
            waveOut.DeviceNumber = GetDeviceNumber();
            waveOut.Init(provider);
            waveOut.Play();
            */

            //var sound = new InMemorySound(format, audioData);

            //var provider = new InMemorySoundSampleProvider(sound);

            //var provider = new RawSourceWaveStream(audioData, 0, audioData.Length, format);

            var resamplingProvider = new WdlResamplingSampleProvider(provider, _mixer.WaveFormat.SampleRate);

            return AddMixerInput(resamplingProvider);
        }

        public void StopSound(ISampleProvider provider)
        {
            _mixer.RemoveMixerInput(provider);
        }

        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == _mixer.WaveFormat.Channels)
            {
                return input;
            }

            if (input.WaveFormat.Channels == 1 && _mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(input);
            }

            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        private ISampleProvider AddMixerInput(ISampleProvider input)
        {
            var mixerInput = ConvertToRightChannelCount(input);

            _mixer.AddMixerInput(mixerInput);

            return mixerInput;
        }

        public void Dispose()
        {
            _mixer.RemoveAllMixerInputs();

            _outputDevice.Dispose();
        }

        public static readonly AudioPlaybackEngine Instance = new AudioPlaybackEngine(44100, 2);
    }
}
