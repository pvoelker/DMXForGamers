using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Models
{
    public class FileMonitor : ObservableValidator
    {
        public FileMonitor()
        {
            ValidateAllProperties();
        }

        private string m_FilePath;

        [Required(ErrorMessage = "File Path is Required")]
        public string FilePath
        {
            get => m_FilePath;
            set => SetProperty(ref m_FilePath, value, true);
        }
    }
}
