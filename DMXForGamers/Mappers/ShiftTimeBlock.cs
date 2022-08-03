using System;
using System.Collections.Generic;

namespace DMXForGamers.Mappers
{
    public class ShiftTimeBlock
    {
        public ViewModels.ShiftTimeBlock ToModel(Models.DMXTimeBlock data)
        {
            var retVal = new ViewModels.ShiftTimeBlock
            {
                StartTime = data.StartTime,
                TimeSpan = data.TimeSpan,
            };

            return retVal;
        }

        public void UpdateFromModel(ViewModels.ShiftTimeBlock data, Models.DMXTimeBlock dataToUpdate)
        {
            dataToUpdate.StartTime = data.NewStartTime.HasValue ? data.NewStartTime.Value : dataToUpdate.StartTime;
        }
    }
}
