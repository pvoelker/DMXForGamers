using System;

namespace DMXCommunication
{
	/// <summary>
	/// Provides a no-operation implementation of the IDMXCommunication interface that performs no actions and does not
	/// communicate with any hardware.
	/// </summary>
	/// <remarks>Use NullAdapter when a DMX communication interface is required but no actual hardware interaction
	/// is needed. This can be useful for testing, development, or as a default placeholder in scenarios where DMX output
	/// is optional.</remarks>
	public class NullAdapter : IDMXCommunication
	{
        static public Guid ID = new Guid("1c01e3c1-ef23-4285-87b6-bd220610c6d7");
        public Guid Identifier { get { return ID; } }
		public string Description { get { return "Null Adapter"; } }

        public object Settings
        {
            get { return null; }
        }

        object IDMXCommunication.Settings { get => Settings; set => throw new NotImplementedException(); }

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

