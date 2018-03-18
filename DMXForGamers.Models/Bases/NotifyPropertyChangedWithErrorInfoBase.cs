using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DMXForGamers.Models
{
    public abstract class NotifyPropertyChangedWithErrorInfoBase : NotifyPropertyChangedBase, IDataErrorInfo
    {
        public NotifyPropertyChangedWithErrorInfoBase() : base()
        {
        }

        #region IDataErrorInfo implementation

        public abstract string this[string columnName] { get; }

        public string Error { get { return this[null]; } }

        #endregion
    }
}
