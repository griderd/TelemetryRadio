using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelemetryRadio.Protocols
{
    public abstract class PacketDecoder
    {
        protected int byteOffset;

        protected List<byte> buffer;

        public bool EndOfPacket { get; protected set; }
        public bool PacketIsValid { get; protected set; }
        public bool CanRead { get; protected set; }

        /// <summary>
        /// The current offset of the decoder from the front of the pseudoheader
        /// </summary>
        public int ByteOffset
        {
            get
            {
                return byteOffset;
            }
        }

        /// <summary>
        /// The current offset of the decoder from the front of the data
        /// </summary>
        public abstract int Offset { get; }

        public PacketDecoder()
        {
            byteOffset = 0;
            buffer = new List<byte>();
            EndOfPacket = false;
            PacketIsValid = false;
            CanRead = false;
        }

        /// <summary>
        /// Converts the current data to a packet.
        /// </summary>
        /// <param name="packet">Location to save the packet.</param>
        /// <returns>If the conversion succeeds, returns true. Otherwise returns false.</returns>
        public abstract bool ToPacket(out IPacket packet);
       
        /// <summary>
        /// Adds a set of bytes to decode, decodes them, and outputs any unneeded bytes.
        /// </summary>
        /// <param name="b">Bytes to decode</param>
        /// <param name="overflow">Location to send overflow bytes</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of bytes.</exception>
        public abstract void AddBytes(byte[] b, out byte[] overflow);

        /// <summary>
        /// Advances the buffer by the given number of bytes, and increments the byte offset by the same amount.
        /// </summary>
        /// <param name="count"></param>
        protected void Advance(int count)
        {
            byteOffset += count;
            buffer.RemoveRange(0, count);
        }

        public bool TryReadByte(out byte value)
        {
            if (CanRead) return ReadByte(out value);

            value = 0;
            return false;
        }

        protected bool ReadByte(out byte value)
        {
            if (buffer.Count >= 1)
            {
                value = buffer[0];
                Advance(1);
                return true;
            }
            value = 0;
            return false;
        }

        public bool TryReadChar(out char value)
        {
            if (CanRead) return ReadChar(out value);

            value = '\0';
            return false;
        }

        protected bool ReadChar(out char value)
        {
            if (buffer.Count >= 1)
            {
                value = Encoding.UTF8.GetChars(buffer.ToArray(), 0, 1)[0];
                Advance(1);
                return true;
            }
            value = '\0';
            return false;
        }

        protected bool TryReadChars(int count, out char[] value)
        {
            if (CanRead) return ReadChars(count, out value);

            value = new char[0];
            return false;
        }

        protected bool ReadChars(int count, out char[] value)
        {
            if (buffer.Count >= count)
            {
                value = Encoding.UTF8.GetChars(buffer.ToArray(), 0, count);
                Advance(1);
                return true;
            }
            value = new char[0];
            return false;
        }

        public bool TryReadString(int length, out string value)
        {
            if (CanRead) return ReadString(length, out value);

            value = "";
            return false;
        }

        protected bool ReadString(int length, out string value)
        {
            char[] str;

            if (ReadChars(length, out str))
            {
                value = new string(str);
                return true;
            }
            value = "";
            return false;
        }

        public bool TryReadUInt16(out ushort value)
        {
            if (CanRead) return ReadUInt16(out value);

            value = 0;
            return false;
        }

        protected bool ReadUInt16(out ushort value)
        {
            if (buffer.Count >= 2)
            {
                value = BitConverter.ToUInt16(buffer.ToArray(), 0);
                Advance(2);
                return true;
            }
            value = 0;
            return false;
        }

        public bool TryReadInt16(out short value)
        {
            if (CanRead) return ReadInt16(out value);

            value = 0;
            return false;
        }

        protected bool ReadInt16(out short value)
        {
            if (buffer.Count >= 2)
            {
                value = BitConverter.ToInt16(buffer.ToArray(), 0);
                Advance(2);
                return true;
            }
            value = 0;
            return false;
        }

        public bool TryReadUInt32(out uint value)
        {
            if (CanRead) return ReadUInt32(out value);

            value = 0;
            return false;
        }

        protected bool ReadUInt32(out uint value)
        {
            if (buffer.Count >= 4)
            {
                value = BitConverter.ToUInt32(buffer.ToArray(), 0);
                Advance(4);
                return true;
            }
            value = 0;
            return false;
        }

        public bool TryReadInt32(out int value)
        {
            if (CanRead) return ReadInt32(out value);

            value = 0;
            return false;
        }

        protected bool ReadInt32(out int value)
        {
            if (buffer.Count >= 4)
            {
                value = BitConverter.ToInt32(buffer.ToArray(), 0);
                Advance(4);
                return true;
            }
            value = 0;
            return false;
        }

        public bool TryReadFloat(out float value)
        {
            if (CanRead) return ReadFloat(out value);

            value = 0;
            return false;
        }

        protected bool ReadFloat(out float value)
        {
            if (buffer.Count >= 4)
            {
                value = BitConverter.ToSingle(buffer.ToArray(), 0);
                Advance(4);
                return true;
            }
            value = 0;
            return false;
        }

        public bool TryReadUInt64(out ulong value)
        {
            if (CanRead) return ReadUInt64(out value);

            value = 0;
            return false;
        }

        protected bool ReadUInt64(out ulong value)
        {
            if (buffer.Count >= 8)
            {
                value = BitConverter.ToUInt64(buffer.ToArray(), 0);
                Advance(8);
                return true;
            }
            value = 0;
            return false;
        }

        public bool TryReadInt64(out long value)
        {
            if (CanRead) return ReadInt64(out value);

            value = 0;
            return false;
        }

        protected bool ReadInt64(out long value)
        {
            if (buffer.Count >= 8)
            {
                value = BitConverter.ToInt64(buffer.ToArray(), 0);
                Advance(8);
                return true;
            }
            value = 0;
            return false;
        }

        public bool TryReadDouble(out double value)
        {
            if (CanRead) return ReadDouble(out value);

            value = 0;
            return false;
        }

        protected bool ReadDouble(out double value)
        {
            if (buffer.Count >= 8)
            {
                value = BitConverter.ToDouble(buffer.ToArray(), 0);
                Advance(8);
                return true;
            }
            value = 0;
            return false;
        }
    }
}
