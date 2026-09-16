using System;

namespace DMXCommunication
{
    /// <summary>
    /// Defines the contract for DMX communication interfaces, providing methods to control DMX data transmission and
    /// manage channel values.
    /// </summary>
    public interface IDMXCommunication : IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for the DMX communication interface type.
        /// </summary>
        Guid Identifier { get; }
        /// <summary>
        /// Gets the description associated with the DMX communication interface type.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Begins DMX data communication.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops DMX data communication.
        /// </summary>
        void Stop();

        /// <summary>
        /// Clears all DMX channel values, resetting them to their default state (zero).
        /// </summary>
        void ClearChannelValues();

        /// <summary>
        /// Sets the value of a specific DMX channel.
        /// </summary>
        /// <param name="channel">The DMX channgel to set. Valid value are 0 - 512.</param>
        /// <param name="value">The DMX value to set.</param>
        void SetChannelValue(ushort channel, byte value);

        /// <summary>
        /// Gets or sets the settings object associated with this instance.
        /// </summary>
        /// <remarks>The specific type and structure of the settings object depend on the implementation.
        /// Consumers should refer to the documentation of the implementing class for details on supported settings and
        /// expected object types.</remarks>
        object Settings { get; set; }
    }
}