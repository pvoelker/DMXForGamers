using System;
using System.Collections.Generic;

namespace DMXForGamers.Mappers
{
    public class ShiftTimeBlock
    {
        public Models.ShiftTimeBlock ToModel(Models.DMXTimeBlock data)
        {
            var retVal = new Models.ShiftTimeBlock
            {
                Id = data.Id,
                StartTime = data.StartTime,
                TimeSpan = data.TimeSpan,
            };

            return retVal;
        }

        public void UpdateFromModel(Models.ShiftTimeBlock data, Models.DMXTimeBlock dataToUpdate)
        {
            dataToUpdate.StartTime = data.NewStartTime.HasValue ? data.NewStartTime.Value : dataToUpdate.StartTime;
        }
    }
}
