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

            foreach(var item in data.BaseDMXValues)
            {
                retVal.BaseDMXValues.Add(DMXValue.ToModel(item));
            }

            foreach (var item in data.Events)
            {
                retVal.Events.Add(DMXEvent.ToModel(item));
            }

            return retVal;
        }

        public static DMXEngine.DMX FromModel(Models.DMXDefinitions data)
        {
            var retVal = new DMXEngine.DMX()
            {
                Description = data.Description,
                AllowOneActiveEvent = data.AllowOneActiveEvent
            };

            foreach (var item in data.BaseDMXValues)
            {
                retVal.BaseDMXValues.Add(DMXValue.FromModel(item));
            }

            foreach (var item in data.Events)
            {
                retVal.Events.Add(DMXEvent.FromModel(item));
            }

            return retVal;
        }
    }
}
