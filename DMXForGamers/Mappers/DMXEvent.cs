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

            foreach (var item in data.TimeBlocks)
            {
                retVal.TimeBlocks.Add(DMXTimeBlock.ToModel(item));
            }

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

            foreach (var item in data.TimeBlocks)
            {
                retVal.TimeBlocks.Add(DMXTimeBlock.FromModel(item));
            }

            return retVal;
        }
    }
}
