using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Models
{
    public class DMXProtocol : NotifyPropertyChangedBase
    {
        public DMXProtocol(Guid id, string name, Type type)
        {
            ID = id;
            Name = name;
            Type = type;
        }

        private Guid m_ID;
        public Guid ID
        {
            get { return m_ID; }
            set { m_ID = value; AnnouncePropertyChanged(); }
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; AnnouncePropertyChanged(); }
        }

        private Type m_Type;
        public Type Type
        {
            get { return m_Type; }
            set { m_Type = value; AnnouncePropertyChanged(); }
        }
    }
}
