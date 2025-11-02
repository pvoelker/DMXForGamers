using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXTimeBlock
    {
        public static Models.DMXTimeBlock ToModel(DMXEngine.TimeBlock data)
        {
            var retVal = new Models.DMXTimeBlock
            {
                StartTime = data.StartTime,
                TimeSpan = data.TimeSpan,
            };

            retVal.DMXValues.AddRange(data.DMXValues.Select(DMXValue.ToModel));

            return retVal;
        }

        public static DMXEngine.TimeBlock FromModel(Models.DMXTimeBlock data)
        {
            var retVal = new DMXEngine.TimeBlock
            {
                StartTime = data.StartTime,
                TimeSpan = data.TimeSpan,
            };

            retVal.DMXValues.AddRange(data.DMXValues.Select(DMXValue.FromModel));

            return retVal;
        }
    }
}
