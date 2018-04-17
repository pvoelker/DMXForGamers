using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Models
{
    public class NewProcessMonitor : NotifyPropertyChangedWithErrorInfoBase
    {
        private string m_ExeFile;
        public string ExeFilePath
        {
            get { return m_ExeFile; }
            set { m_ExeFile = value; AnnouncePropertyChanged(); }
        }

        private string m_ExeArgs;
        public string ExeArgs
        {
            get { return m_ExeArgs; }
            set { m_ExeArgs = value; AnnouncePropertyChanged(); }
        }

        #region IDataErrorInfo

        public override string this[string columnName]
        {
            get
            {
                var errorStr = new StringBuilder();

                if ((columnName == nameof(ExeFilePath)) || (columnName == null))
                {
                    if (String.IsNullOrWhiteSpace(ExeFilePath) == true)
                    {
                        errorStr.AppendLine("EXE File Path is required");
                    }
                }

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion
    }
}
