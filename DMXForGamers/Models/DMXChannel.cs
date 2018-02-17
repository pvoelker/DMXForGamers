using System;

namespace DMXForGamers.Models
{
    public class DMXChannel : NotifyPropertyChangedBase
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
            get { return _channel; }
            set { _channel = value; AnnouncePropertyChanged(); }
        }

        private byte _value = 0;
        public byte Value
        {
            get { return _value; }
            set { _value = value; AnnouncePropertyChanged(); }
        }

        public override string ToString()
        {
            return String.Format("Channel = {0}, Value = {1}", this.Channel, this.Value);
        }
    }
}
