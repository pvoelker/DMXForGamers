using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXValue : NotifyPropertyChangedWithErrorInfoBase
    {
        private ushort _channel;
        public ushort Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        private byte _value;
        public byte Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private short _delta;
        public short Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private ICommand _delete;
        public ICommand Delete
        {
            get { return _delete; }
            set { _delete = value; AnnouncePropertyChanged(); }
        }

        #region IErrorInfo

        public override string this[string columnName]
        {
            get
            {
                var errorStr = new StringBuilder();

                if ((columnName == nameof(Channel)) || (columnName == null))
                {
                    if ((Channel < 1) || (Channel > 512))
                    {
                        errorStr.AppendLine("DMX Channels Must Be Between 1 and 512 Inclusive");
                    }
                }
                if ((columnName == nameof(Delta)) || (columnName == null))
                {
                    if ((Delta < -255) || (Delta > 255))
                    {
                        errorStr.AppendLine("Delta Must Be Between -255 and 255 Inclusive");
                    }
                }

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion
    }
}
