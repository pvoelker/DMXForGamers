using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        public IEnumerable<string> Errors
        {
            get
            {
                var errors = this[null];

                if (errors == null)
                    return new List<string>();
                else
                    return this[null].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        virtual public IEnumerable<string> Validate()
        {
            return Errors;
        }
    }
}
