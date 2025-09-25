using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DMXEngine
{
    public static class AudioHelpers
    {
        public static WaveStream GetWaveStream(string fileExt, byte[] soundData)
        {
            if (fileExt.ToLower() == ".wav")
            {
                return new WaveFileReader(new MemoryStream(soundData));
            }
            else if (fileExt.ToLower() == ".mp3")
            {
                return new Mp3FileReader(new MemoryStream(soundData));
            }
            else
            {
                throw new Exception($"Unknown file format ({fileExt})");
            }
        }
    }
}
