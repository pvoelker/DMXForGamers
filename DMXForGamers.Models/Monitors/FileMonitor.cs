using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Models
{
    public class FileMonitor : NotifyPropertyChangedWithErrorInfoBase
    {
        private string m_FilePath;
        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; AnnouncePropertyChanged(); }
        }

        #region IDataErrorInfo

        public override string this[string columnName]
        {
            get
            {
                var errorStr = new StringBuilder();

                if ((columnName == nameof(FilePath)) || (columnName == null))
                {
                    if(String.IsNullOrWhiteSpace(FilePath) == true)
                    {
                        errorStr.AppendLine("File Path is required");
                    }
                }

                return (errorStr.Length == 0) ? null : errorStr.ToString();
            }
        }

        #endregion
    }
}
