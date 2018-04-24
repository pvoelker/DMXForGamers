using System;

namespace DMXCommunication
{
    public class ArtNetMessage
    {
        public ArtNetMessage(ushort universe, byte[] dmxValues)
        {
            Universe = universe;
            DMXValues = dmxValues;
        }

        public ushort Universe { get; set; }

        public byte[] DMXValues { get; set; }

        public byte[] ToByteArray()
        {
            if (DMXValues.Length != 512)
                throw new Exception("'DMXValues' must have a lenth of 512");

            var bytes = new byte[530];

            bytes[0] = (byte)'A';
            bytes[1] = (byte)'r';
            bytes[2] = (byte)'t';
            bytes[3] = (byte)'-';
            bytes[4] = (byte)'N';
            bytes[5] = (byte)'e';
            bytes[6] = (byte)'t';
            bytes[7] = 0;

            // Opcode (ArtDMX)
            bytes[8] = 0x00;
            bytes[9] = 0x50;

            // Protocol Version
            bytes[10] = 0;
            bytes[11] = 14;

            // Sequence (not used)
            bytes[12] = 0;

            // Physical Port (informational only and hard coded to 0)
            bytes[13] = 0;

            // Universe/Port-Address
            var universeBytes = UShortToByteArray(Universe);
            bytes[14] = universeBytes[0];
            bytes[15] = universeBytes[1];

            // Length (512)
            var lengthBytes = UShortToByteArray(512);
            bytes[16] = lengthBytes[0];
            bytes[17] = lengthBytes[1];

            Buffer.BlockCopy(DMXValues, 0, bytes, 18, 512);

            return bytes;
        }

        private byte[] UShortToByteArray(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }
    }
}
