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

        private ICommand _deleteDMXValue;
        public ICommand DeleteDMXValue
        {
            get { return _deleteDMXValue; }
            set { _deleteDMXValue = value; AnnouncePropertyChanged(); }
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
                if ((columnName == nameof(Delta)) || (columnName == nameof(Value)) || (columnName == null))
                {
                    var total = Value + Delta;
                    if ((total < -255) || (total > 255))
                    {
                        errorStr.AppendLine("Total of Value and Delta Values Must Be Between -255 and 255 Inclusive");
                    }
                }

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion
    }
}
