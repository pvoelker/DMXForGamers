using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXDefinitions
    {
        public Models.DMXDefinitions ToModel(DMXEngine.DMX data)
        {
            var retVal = new Models.DMXDefinitions()
            {
                Description = data.Description,
                AllowOneActiveEvent = data.AllowOneActiveEvent
            };

            var dmxMapper = new DMXValue();
            foreach(var item in data.BaseDMXValues)
            {
                retVal.BaseDMXValues.Add(dmxMapper.ToModel(item));
            }

            var eventMapper = new DMXEvent();
            foreach (var item in data.Events)
            {
                retVal.Events.Add(eventMapper.ToModel(item));
            }

            return retVal;
        }

        public DMXEngine.DMX FromModel(Models.DMXDefinitions data)
        {
            var retVal = new DMXEngine.DMX()
            {
                Description = data.Description,
                AllowOneActiveEvent = data.AllowOneActiveEvent
            };

            var dmxMapper = new DMXValue();
            foreach (var item in data.BaseDMXValues)
            {
                retVal.BaseDMXValues.Add(dmxMapper.FromModel(item));
            }

            var eventMapper = new DMXEvent();
            foreach (var item in data.Events)
            {
                retVal.Events.Add(eventMapper.FromModel(item));
            }

            return retVal;
        }
    }
}
