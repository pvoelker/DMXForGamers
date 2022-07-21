using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace DMXForGamers.Models
{
    public class DMXProtocol : ObservableValidator
    {
        public DMXProtocol(Guid id, string description, Type type)
        {
            ID = id;
            Description = description;
            Type = type;

            ValidateAllProperties();
        }

        private Guid m_ID;
        public Guid ID
        {
            get { return m_ID; }
            set { SetProperty(ref m_ID, value, true); }
        }

        private string m_Description;
        public string Description
        {
            get { return m_Description; }
            set { SetProperty(ref m_Description, value, true); }
        }

        private Type m_Type;
        public Type Type
        {
            get { return m_Type; }
            set { SetProperty(ref m_Type, value, true); }
        }

        private object m_Settings;
        public object Settings
        {
            get { return m_Settings; }
            set { SetProperty(ref m_Settings, value, true); }
        }
    }
}
