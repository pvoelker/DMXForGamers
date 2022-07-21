using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXTimeBlock
    {
        public Models.DMXTimeBlock ToModel(DMXEngine.TimeBlock data)
        {
            var retVal = new Models.DMXTimeBlock
            {
                StartTime = data.StartTime,
                TimeSpan = data.TimeSpan,
            };

            var mapper = new DMXValue();
            foreach(var item in data.DMXValues)
            {
                retVal.DMXValues.Add(mapper.ToModel(item));
            }

            return retVal;
        }

        public DMXEngine.TimeBlock FromModel(Models.DMXTimeBlock data)
        {
            var retVal = new DMXEngine.TimeBlock
            {
                StartTime = data.StartTime,
                TimeSpan = data.TimeSpan,
            };

            var mapper = new DMXValue();
            foreach (var item in data.DMXValues)
            {
                retVal.DMXValues.Add(mapper.FromModel(item));
            }

            return retVal;
        }
    }
}
