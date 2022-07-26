using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Models
{
    public class NewProcessMonitor : ObservableValidator
    {
        public NewProcessMonitor()
        {
            ValidateAllProperties();
        }

        private string m_ExeFile;
        [Required(ErrorMessage = "EXE File Path is Required")]
        public string ExeFilePath
        {
            get => m_ExeFile;
            set => SetProperty(ref m_ExeFile, value, true);
        }

        private string m_ExeArgs;
        public string ExeArgs
        {
            get => m_ExeArgs;
            set => SetProperty(ref m_ExeArgs, value, true);
        }
    }
}
