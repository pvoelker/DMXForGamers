using System;

namespace DMXForGamers.Mappers
{
    public class DMXProtocol
    {
        public static Models.DMXProtocol ToModel(DMXCommunication.Models.DMXPortAdapter data)
        {
            return new Models.DMXProtocol(data.ID, data.Description, data.Type)
            {
                Settings = data.Settings
            };
        }
    }
}
