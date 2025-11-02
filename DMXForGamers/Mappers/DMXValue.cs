using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXValue
    {
        public static Models.DMXValue ToModel(DMXEngine.DMXValue data)
        {
            return new Models.DMXValue
            {
                Channel = data.Channel,
                Value = data.Value,
                Delta = data.Delta
            };
        }

        public static DMXEngine.DMXValue FromModel(Models.DMXValue data)
        {
            return new DMXEngine.DMXValue
            {
                Channel = data.Channel,
                Value = data.Value,
                Delta = data.Delta
            };
        }
    }
}
