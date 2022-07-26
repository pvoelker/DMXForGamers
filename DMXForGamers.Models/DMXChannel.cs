using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace DMXForGamers.Models
{
    public class DMXChannel : ObservableObject
    {
        public DMXChannel(ushort channel)
        {
            Channel = channel;
        }

        public DMXChannel(ushort channel, byte value)
        {
            Channel = channel;
            Value = value;
        }

        private ushort _channel;
        public ushort Channel
        {
            get => _channel;
            set => SetProperty(ref _channel, value);
        }

        private byte _value = 0;
        public byte Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public override string ToString()
        {
            return String.Format("Channel = {0}, Value = {1}", Channel, Value);
        }
    }
}
