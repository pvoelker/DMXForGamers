using System;

namespace DMXCommunication
{
	public interface IDMXCommunication : IDisposable
	{
		Guid Identifier { get; }
		string Description { get; }

		void Start ();

		void Stop ();

		void ClearChannelValues ();

		void SetChannelValue (ushort channel, byte value);
	}
}