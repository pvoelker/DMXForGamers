using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DMXForGamers.Models
{
    public class DMXValue : NotifyPropertyChangedWithErrorInfoBase
    {
        public IEnumerable<DMXValue> ParentCollection { get; set; }

        private ushort _channel;
        public ushort Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }
        static public ushort MinChannel { get { return 1; } }
        static public ushort MaxChannel { get { return 512; } }

        private byte _value;
        public byte Value
        {
            get { return _value; }
            set { _value = value; }
        }
        static public short MinValue { get { return 0; } }
        static public short MaxValue { get { return 255; } }

        private short _delta;
        public short Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }
        static public short MinDelta { get { return -255; } }
        static public short MaxDelta { get { return 255; } }

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
                    if ((Channel < MinChannel) || (Channel > MaxChannel))
                    {
                        errorStr.AppendLine(String.Format("DMX Channels Must Be Between {0} and {1} Inclusive", MinChannel, MaxChannel));
                    }
                    else
                    {
                        if (ParentCollection != null)
                        {
                            var duplicateCount = ParentCollection.Where(x => x != this).Count(x => x.Channel == this.Channel);
                            if (duplicateCount > 0)
                            {
                                errorStr.AppendLine("Channel is duplicated");
                            }
                        }
                    }
                }
                if ((columnName == nameof(Delta)) || (columnName == null))
                {
                    if ((Delta < -255) || (Delta > 255))
                    {
                        errorStr.AppendLine(String.Format("Delta Must Be Between {0} and {1} Inclusive", MinDelta, MaxDelta));
                    }
                }
                if ((columnName == nameof(Delta)) || (columnName == nameof(Value)) || (columnName == null))
                {
                    var total = Value + Delta;
                    if ((total < MinValue) || (total > MaxValue))
                    {
                        errorStr.AppendLine(String.Format("Total of Value and Delta Values Must Be Between {0} and {1} Inclusive", MinValue, MaxValue));
                    }
                }

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion
    }
}
