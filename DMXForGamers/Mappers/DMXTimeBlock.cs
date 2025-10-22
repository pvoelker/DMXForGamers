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

            foreach(var item in data.DMXValues)
            {
                retVal.DMXValues.Add(DMXValue.ToModel(item));
            }

            return retVal;
        }

        public static DMXEngine.TimeBlock FromModel(Models.DMXTimeBlock data)
        {
            var retVal = new DMXEngine.TimeBlock
            {
                StartTime = data.StartTime,
                TimeSpan = data.TimeSpan,
            };

            foreach (var item in data.DMXValues)
            {
                retVal.DMXValues.Add(DMXValue.FromModel(item));
            }

            return retVal;
        }
    }
}
