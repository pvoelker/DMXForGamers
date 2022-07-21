using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMXForGamers.Mappers
{
    public class DMXEvent
    {
        public Models.DMXEvent ToModel(DMXEngine.Event data)
        {
            var retVal = new Models.DMXEvent()
            {
                EventID = data.EventID,
                TimeSpan = data.TimeSpan,
                RepeatCount = data.RepeatCount,
                SoundFileName = data.SoundFileName,
                SoundData = data.SoundData
            };

            var mapper = new DMXTimeBlock();
            foreach (var item in data.TimeBlocks)
            {
                retVal.TimeBlocks.Add(mapper.ToModel(item));
            }

            return retVal;
        }

        public DMXEngine.Event FromModel(Models.DMXEvent data)
        {
            var retVal = new DMXEngine.Event()
            {
                EventID = data.EventID,
                TimeSpan = data.TimeSpan,
                RepeatCount = data.RepeatCount,
                SoundFileName = data.SoundFileName,
                SoundData = data.SoundData
            };

            var mapper = new DMXTimeBlock();
            foreach (var item in data.TimeBlocks)
            {
                retVal.TimeBlocks.Add(mapper.FromModel(item));
            }

            return retVal;
        }
    }
}
