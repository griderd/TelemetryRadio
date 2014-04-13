using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSLib.Collections;
using GSLib.Extensions;

namespace TelemetryRadio.Protocols.UDP
{
    public class UDPPacketDecoder : PacketDecoder 
    {
        StringBuilder pseudoheader;
        ushort source, dest, length;
        short checksum;

        /// <summary>
        /// The current offset of the decoder from the front of the data
        /// </summary>
        public override int Offset
        {
            get
            {
                return byteOffset - 12;
            }
        }

        public UDPPacketDecoder()
            : base()
        {
            pseudoheader = new StringBuilder();
        }

        public override bool ToPacket(out IPacket packet)
        {
            if (EndOfPacket & PacketIsValid)
            {
                packet = new UDPPacket(dest, source, buffer.ToArray());
                return true;
            }
            else
            {
                packet = new UDPPacket();
                return false;
            }
        }

        /// <summary>
        /// Adds a set of bytes to decode
        /// </summary>
        /// <param name="b"></param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of bytes.</exception>
        public override void AddBytes(byte[] b, out byte[] overflow)
        {
            if (b == null)
                throw new ArgumentNullException();
            
            buffer.AddRange(b);

            // Decode
            while ((byteOffset <= 12) & (buffer.Count > 0))
            {
                switch (byteOffset)
                {
                    case 0:
                        if (buffer.Count >= 5)
                            pseudoheader.Append(Encoding.UTF8.GetChars(buffer.ToArray(), 0, 5));

                        if (pseudoheader.ToString() != UDPPacket.PSEUDOHEADER)
                        {
                            pseudoheader.Clear();
                            buffer.RemoveAt(0);
                        }
                        else
                        {
                            Advance(5);
                        }
                        break;

                    case 5:
                        ReadUInt16(out source);
                        break;

                    case 7:
                        ReadUInt16(out dest);
                        break;

                    case 9:
                        ReadUInt16(out length);
                        break;

                    case 11:
                        ReadInt16(out checksum);
                        break;
                }
            }
            
            overflow = new byte[0];

            CanRead = byteOffset > 12;
            UDPHeader udp = new UDPHeader();

            if ((byteOffset >= 12) && (buffer.Count + udp.HeaderLength >= length))
            {
                if (buffer.Count + udp.HeaderLength > length)
                {
                    overflow = buffer.GetRange(length -  udp.HeaderLength, buffer.Count - length).ToArray();
                    buffer.RemoveRange(length, buffer.Count - length);
                }
                EndOfPacket = true;
                PacketIsValid = (checksum == (new UDPHeader().GenerateChecksum(buffer.ToArray())));
            }
        }

        
    }

    public class UDPPacketBuilder : PacketBuilder
    {
        UDPHeader header;

        public UDPPacketBuilder(ushort destPort, ushort sourcePort = 0)
            : base()
        {
            header = new UDPHeader(destPort, sourcePort);
        }

        public override IPacket ToPacket()
        {
            UDPPacket p = new UDPPacket(header, body.ToArray());
            return p;
        }
    }

    public struct UDPPacket : IPacket 
    {
        internal UDPHeader header;
        internal byte[] data;

        public const string PSEUDOHEADER = "$UDP$";

        public UDPPacket(ushort source, ushort dest, byte[] data)
        {
            this.data = data;
            header = new UDPHeader(dest, source);
            header.SetParameters(data);
        }

        public UDPPacket(ushort dest, byte[] data)
        {
            this.data = data;
            header = new UDPHeader(dest);
            header.SetParameters(data);
        }

        public UDPPacket(UDPHeader header, byte[] data)
        {
            this.header = header;
            this.data = data;
            this.header.SetParameters(data);
        }

        public IHeader Header
        {
            get
            {
                return header;
            }
        }

        public byte[] Body
        {
            get
            {
                return data;
            }
        }

        public int MaxLength
        {
            get
            {
                return (int)ushort.MaxValue;
            }
        }

        public byte[] ToByteArray()
        {
            List<byte> b = new List<byte>();

            b.AddRange(Encoding.UTF8.GetBytes(PSEUDOHEADER));
            b.AddRange(header.ToByteArray());
            b.AddRange(data);

            return b.ToArray();
        }

    }

    public struct UDPHeader : IHeader 
    {
        ushort sourcePort;
        ushort destPort;
        internal ushort length;
        internal short checksum;

        public int HeaderLength
        {
            get
            {
                return 8;
            }
        }

        public ushort SourcePort
        {
            get
            {
                return sourcePort;
            }
            set
            {
                sourcePort = value;
            }
        }

        public ushort DestPort
        {
            get
            {
                return destPort;
            }
            set
            {
                if (destPort == 0)
                    throw new ArgumentOutOfRangeException();

                destPort = value;
            }
        }

        public ushort Length
        {
            get
            {
                return length;
            }
        }

        public short Checksum
        {
            get
            {
                return checksum;
            }
        }

        public UDPHeader(ushort dest, ushort source = 0)
        {
            destPort = dest;
            sourcePort = source;
            length = 0;
            checksum = 0;
        }

        public short GenerateChecksum(byte[] data)
        {
            Bitfield a, b;

            a = Bitfield.FromInt16(0);
            b = Bitfield.FromUInt16(sourcePort);
            a = Add(a, b);
            b = Bitfield.FromUInt16(destPort);
            a = Add(a, b);
            b = Bitfield.FromInt16((short)data.Length);
            a = Add(a, b);

            for (int i = 0; i < data.Length - 1; i++)
            {
                b = Bitfield.FromByte(data[i++]);
                b.Append(Bitfield.FromByte(data[i]));
                a = Add(a, b);
            }

            if (a.SequenceEqual(Bitfield.FromInt16(0)))
                a = Bitfield.FromUInt16(UInt16.MaxValue);

            return a.ToInt16();
        }

        /// <summary>
        /// Sets the length and checksum based on the data provided.
        /// </summary>
        /// <param name="data"></param>
        public void SetParameters(byte[] data)
        {
            checksum = GenerateChecksum(data);
            if (data.Length + 8 > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("UDP supports a maximum data size of 65,527 bytes.");
            length = (ushort)(data.Length + 8);
        }

        private Bitfield Add(Bitfield a, Bitfield b)
        {
            bool carry = false;
            bool willCarry = false;

            for (int i = 0; i < 16; i++)
            {
                willCarry = a[i] & b[i];
                a[i] = a[i] | b[i] | carry;
                carry = willCarry;
            }

            return a;
        }

        public byte[] ToByteArray()
        {
            List<byte> b = new List<byte>();

            b.AddRange(BitConverter.GetBytes(sourcePort));
            b.AddRange(BitConverter.GetBytes(destPort));
            b.AddRange(BitConverter.GetBytes(length));
            b.AddRange(BitConverter.GetBytes(checksum));

            return b.ToArray();
        }
    }


}
