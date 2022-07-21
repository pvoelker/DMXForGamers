using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXValue
    {
        public Models.DMXValue ToModel(DMXEngine.DMXValue data)
        {
            return new Models.DMXValue
            {
                Channel = data.Channel,
                Value = data.Value,
                Delta = data.Delta
            };
        }

        public DMXEngine.DMXValue FromModel(Models.DMXValue data)
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
