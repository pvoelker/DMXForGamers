using System;

namespace DMXForGamers.Models
{
    public class DMXProtocol : NotifyPropertyChangedBase
    {
        public DMXProtocol(Guid id, string description, Type type)
        {
            ID = id;
            Description = description;
            Type = type;
        }

        private Guid m_ID;
        public Guid ID
        {
            get { return m_ID; }
            set { m_ID = value; AnnouncePropertyChanged(); }
        }

        private string m_Description;
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; AnnouncePropertyChanged(); }
        }

        private Type m_Type;
        public Type Type
        {
            get { return m_Type; }
            set { m_Type = value; AnnouncePropertyChanged(); }
        }
    }
}
