using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Web
{
    public class NewClientArgs
    {
        public NewClientArgs(string clientAddress)
        {
            ClientAddress = clientAddress;
        }

        public string ClientAddress { get; set; }
    }
}
