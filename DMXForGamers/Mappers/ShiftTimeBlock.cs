using System;
using System.Collections.Generic;

namespace DMXForGamers.Mappers
{
    public class ShiftTimeBlock
    {
        public static Models.ShiftTimeBlock ToModel(Models.DMXTimeBlock data)
        {
            var retVal = new Models.ShiftTimeBlock
            {
                Id = data.Id,
                StartTime = data.StartTime,
                TimeSpan = data.TimeSpan,
            };

            return retVal;
        }

        public static void UpdateFromModel(Models.ShiftTimeBlock data, Models.DMXTimeBlock dataToUpdate)
        {
            dataToUpdate.StartTime = data.NewStartTime ?? dataToUpdate.StartTime;
        }
    }
}
