using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class EventDefinitions
    {
        public Models.EventDefinitions ToModel(DMXEngine.EventDefinitions data)
        {
            var retVal = new Models.EventDefinitions
            {
                Description = data.Description,
                Notes = data.Notes
            };

            var mapper = new EventDefinition();
            foreach(var item in data.Events)
            {
                retVal.Events.Add(mapper.ToModel(item));
            }

            return retVal;
        }

        public DMXEngine.EventDefinitions FromModel(Models.EventDefinitions data)
        {
            var retVal = new DMXEngine.EventDefinitions
            {
                Description = data.Description,
                Notes = data.Notes
            };

            var mapper = new EventDefinition();
            foreach (var item in data.Events)
            {
                retVal.Events.Add(mapper.FromModel(item));
            }

            return retVal;
        }
    }
}
