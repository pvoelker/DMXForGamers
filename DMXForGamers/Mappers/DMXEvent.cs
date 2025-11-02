using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXEvent
    {
        public static Models.DMXEvent ToModel(DMXEngine.Event data)
        {
            var retVal = new Models.DMXEvent()
            {
                EventID = data.EventID,
                RepeatCount = data.RepeatCount,
                SoundFileName = data.SoundFileName,
                SoundData = data.SoundData
            };

            retVal.TimeBlocks.AddRange(data.TimeBlocks.Select(DMXTimeBlock.ToModel));

            return retVal;
        }

        public static DMXEngine.Event FromModel(Models.DMXEvent data)
        {
            var retVal = new DMXEngine.Event()
            {
                EventID = data.EventID,
                RepeatCount = data.RepeatCount,
                SoundFileName = data.SoundFileName,
                SoundData = data.SoundData
            };

            retVal.TimeBlocks.AddRange(data.TimeBlocks.Select(DMXTimeBlock.FromModel));

            return retVal;
        }
    }
}
