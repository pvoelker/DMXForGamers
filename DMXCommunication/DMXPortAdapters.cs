using DMXCommunication.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMXCommunication
{
    static public class DMXPortAdapterHelpers
    {
        /// <summary>
        /// Retrieves a list of available DMX port adapters implemented in the current assembly.    
        /// </summary>
        /// <remarks>This method discovers all types in the executing assembly that implement the <see
        /// cref="IDMXCommunication"/> interface and creates corresponding <see cref="DMXPortAdapter"/> instances. Each
        /// adapter is initialized with its description, identifier, type, and settings as provided by the communication
        /// implementation.</remarks>
        /// <returns>A list of <see cref="DMXPortAdapter"/> objects representing all detected DMX port adapters. The list is
        /// empty if no adapters are found.</returns>
        static public List<DMXPortAdapter> GetPortAdapters()
        {
            var retVal = new List<DMXPortAdapter>();

            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IDMXCommunication))))
            {
                IDMXCommunication comm = (IDMXCommunication)Activator.CreateInstance(type);

                if (comm != null)
                {
                    retVal.Add(new DMXPortAdapter(comm.Description, comm.Identifier, type, comm.Settings));

                    comm.Dispose();
                }
            }

            return retVal;
        }
    }
}

