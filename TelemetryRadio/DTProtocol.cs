using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelemetryRadio
{
    /// <summary>
    /// Contains functions for transmitting/recieving data in a standard protocol.
    /// </summary>
    public class DTProtocol
    {

    }

    public class DTPacketBuilder
    {
        List<byte> body;
        Guid guid;

        public DTPacketBuilder(RadioTransmitter transmitter)
        {
            body = new List<byte>();
        }

        public void Add(byte value)
        {
            body.Add(value);
        }

        public void Add(byte[] value)
        {
            body.AddRange(value);
        }

        public void Add(short value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(ushort value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(int value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(uint value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(long value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(ulong value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(double value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(float value)
        {
            Add(BitConverter.GetBytes(value));
        }

        public void Add(string value)
        {
            Add(Encoding.UTF8.GetBytes(value));
        }

        public void Add(char value)
        {
            Add(Encoding.UTF8.GetBytes(new char[] { value }));
        }

        public DTPPacket ToPacket()
        {
            if (body.LongCount() <= DTPPacket.MAX_LENGTH)
            {
                return new DTPPacket(guid, body.ToArray());
            }
            else
            {
                return new DTPPacket();
            }
        }
    }

    /// <summary>
    /// Represents a DTProtocol data packet. This is different from a data packet.
    /// </summary>
    public struct DTPPacket
    {
        public DTPHeader header;
        public byte[] body;

        /// <summary>
        /// Represents greatest number of bytes that the body can be.
        /// </summary>
        public const uint MAX_LENGTH = uint.MaxValue - DTPHeader.HEADER_SIZE;

        public DTPPacket(Guid senderId, byte[] data)
        {
            if (data.LongLength + DTPHeader.HEADER_SIZE > uint.MaxValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                header = new DTPHeader((uint)data.LongLength + DTPHeader.HEADER_SIZE, senderId);
                body = data;
            }
        }

        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[header.packetSize];
            header.ToByteArray().CopyTo(bytes, 0);
            body.CopyTo(bytes, DTPHeader.HEADER_SIZE);

            return bytes;
        }
    }

    /// <summary>
    /// Represents a 20-byte packet header containing size and identifier information
    /// </summary>
    public struct DTPHeader
    {
        public const int HEADER_SIZE = 20;

        public uint packetSize;
        public Guid senderId;

        public DTPHeader(uint packetSize, Guid senderId)
        {
            this.packetSize = packetSize;
            this.senderId = senderId;
        }

        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[HEADER_SIZE];
            BitConverter.GetBytes(packetSize).CopyTo(bytes, 0);
            senderId.ToByteArray().CopyTo(bytes, 4);

            return bytes;
        }
    }
}
