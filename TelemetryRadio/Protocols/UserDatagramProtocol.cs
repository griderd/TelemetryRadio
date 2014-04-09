using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSLib.Collections;
using GSLib.Extensions;

namespace TelemetryRadio.Protocols
{
    public class UDPPacketDecoder
    {
        WordOffset offset;
        int startOffset;
        public const int WORD_LENGTH = 2;

        List<byte> data;
        Range<byte> currentWord;

        UDPPacket packet;

        public bool IsValid { get; private set; }
        public bool IsComplete { get; private set; }

        public enum WordOffset
        {
            SourcePort,
            DestPort,
            Length,
            Checksum,
            Data,
            End
        }

        public UDPPacketDecoder()
        {
            offset = WordOffset.SourcePort;
            data = new List<byte>();
            currentWord = new Range<byte>(0, 0, data);
            IsValid = false;
            IsComplete = false;
            packet = new UDPPacket();
            startOffset = 0;
        }

        public void AppendBytes(byte[] arr)
        {
            data.AddRange(arr);
            if (currentWord.Info.Length == 0)
                currentWord.Grow(WORD_LENGTH);
                

            while (currentWord.UpperBoundary < data.Count && !IsValid)
            {
                switch (offset)
                {
                    case WordOffset.SourcePort:
                        ushort sourcePort = 0;
                        while (sourcePort == 0 & currentWord.UpperBoundary < data.Count)
                        {
                            sourcePort = BitConverter.ToUInt16(currentWord.Values, 0);
                            if (sourcePort == 0)
                                currentWord++;
                        }
                        packet.header.SourcePort = sourcePort;
                        currentWord += WORD_LENGTH;
                        offset++;
                        break;

                    case WordOffset.DestPort:
                        packet.header.DestPort = BitConverter.ToUInt16(currentWord.Values, 0);
                        currentWord += WORD_LENGTH;
                        offset++;
                        break;

                    case WordOffset.Length:
                        packet.header.length = BitConverter.ToUInt16(currentWord.Values, 0);
                        currentWord += WORD_LENGTH;
                        offset++;
                        break;

                    case WordOffset.Checksum:
                        packet.header.checksum = BitConverter.ToInt16(currentWord.Values, 0);
                        currentWord += WORD_LENGTH;
                        offset++;
                        break;

                    case WordOffset.Data:
                        try
                        {
                            currentWord.Info = new RangeInfo(currentWord.Offset, packet.header.Length);
                        }
                        catch
                        {
                            IsComplete = true;
                            offset = WordOffset.SourcePort;
                            startOffset++;
                        }

                }
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
