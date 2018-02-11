using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DMXCommunication
{
	public class DMXPortAdapter{
		public DMXPortAdapter(string description, Guid id, Type type)
		{
			Description = description;
			ID = id;
			Type = type;
		}

		public string Description { get; private set; }
		public Guid ID { get; private set; }
		public Type Type { get; private set; }
	}

	static public class DMXPortAdapterHelpers
	{
		static public List<DMXPortAdapter> GetPortAdapters()
		{
			var retVal = new List<DMXPortAdapter> ();

			foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => x .GetInterfaces().Contains(typeof(IDMXCommunication))))
			{
				IDMXCommunication comm = (IDMXCommunication) Activator.CreateInstance (type);

				if (comm != null) {
					retVal.Add (new DMXPortAdapter(comm.Description, comm.Identifier, type));

					comm.Dispose ();
				}
			}

			return retVal;
		}
	}
}

