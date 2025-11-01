using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXDefinitions
    {
        public static Models.DMXDefinitions ToModel(DMXEngine.DMX data)
        {
            var retVal = new Models.DMXDefinitions()
            {
                Description = data.Description,
                AllowOneActiveEvent = data.AllowOneActiveEvent
            };

            retVal.BaseDMXValues.AddRange(data.BaseDMXValues.Select(DMXValue.ToModel));

            retVal.Events.AddRange(data.Events.Select(DMXEvent.ToModel));

            return retVal;
        }

        public static DMXEngine.DMX FromModel(Models.DMXDefinitions data)
        {
            var retVal = new DMXEngine.DMX()
            {
                Description = data.Description,
                AllowOneActiveEvent = data.AllowOneActiveEvent
            };

            retVal.BaseDMXValues.AddRange(data.BaseDMXValues.Select(DMXValue.FromModel));

            retVal.Events.AddRange(data.Events.Select(DMXEvent.FromModel));

            return retVal;
        }
    }
}
