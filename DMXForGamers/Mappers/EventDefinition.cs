using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class EventDefinition
    {
        public Models.EventDefinition ToModel(DMXEngine.EventDefinition data)
        {
            return new Models.EventDefinition
            {
                Description = data.Description,
                EventID = data.EventID,
                UseRegEx = data.UseRegEx,
                Pattern = data.Pattern,
                Continuous = data.Continuous
            };
        }

        public DMXEngine.EventDefinition FromModel(Models.EventDefinition data)
        {
            return new DMXEngine.EventDefinition
            {
                Description = data.Description,
                EventID = data.EventID,
                UseRegEx = data.UseRegEx,
                Pattern = data.Pattern,
                Continuous = data.Continuous
            };
        }
    }
}
