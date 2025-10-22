using DMXForGamers.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXProtocol
    {
        public static Models.DMXProtocol ToModel(DMXCommunication.DMXPortAdapter data)
        {
            return new Models.DMXProtocol(data.ID, data.Description, data.Type)
            {
                Settings = data.Settings
            };
        }
    }
}
