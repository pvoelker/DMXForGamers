using System;
using System.IO.Ports;
using System.Threading;

namespace DMXCommunication
{
	public class NullAdapter : IDMXCommunication
	{
        static public Guid ID = new Guid("1c01e3c1-ef23-4285-87b6-bd220610c6d7");
        public Guid Identifier { get { return ID; } }
		public string Description { get { return "Null Adapter"; } }

		public void Start ()
		{
		}

		public void Stop ()
		{
		}

		public void ClearChannelValues ()
		{
		}


		public void SetChannelValue (ushort channel, byte value)
		{
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		~NullAdapter()
		{
			Dispose (false);
		}

		protected virtual void Dispose (bool disposing)
		{
			Stop ();

			if (disposing) {
				// Free managed resources
			}

			// Free native resources if there are any
		}

		#endregion
	}
}

