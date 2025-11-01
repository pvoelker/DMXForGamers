using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class EventDefinitions
    {
        public static Models.EventDefinitions ToModel(DMXEngine.EventDefinitions data)
        {
            var retVal = new Models.EventDefinitions
            {
                Description = data.Description,
                Notes = data.Notes
            };

            retVal.Events.AddRange(data.Events.Select(EventDefinition.ToModel));

            return retVal;
        }

        public static DMXEngine.EventDefinitions FromModel(Models.EventDefinitions data)
        {
            var retVal = new DMXEngine.EventDefinitions
            {
                Description = data.Description,
                Notes = data.Notes
            };

            retVal.Events.AddRange(data.Events.Select(EventDefinition.FromModel));

            return retVal;
        }
    }
}
