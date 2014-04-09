using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelemetryRadio
{
    public class DataStream
    {
        Queue<byte[]> buffer;
        int packetSize;

        public bool CanRead
        {
            get
            {
                return buffer.Count > 0;
            }
        }

        public DataStream(int packetSize)
        {
            if (packetSize <= 0)
                throw new ArgumentOutOfRangeException();

            this.packetSize = packetSize;
            buffer = new Queue<byte[]>();
        }

        public void Write(byte[] packets)
        {
            if ((packets.Length > 0) && (packets.Length % packetSize == 0))
            {
                int packetCount = packets.Length / packetSize;

                for (int i = 0; i < packetCount; i++)
                {
                    buffer.Enqueue(ExtractPacket(packets, i));
                }
            }
        }

        private byte[] ExtractPacket(byte[] packets, int packetIndex)
        {
            byte[] packet = new byte[packetSize];
            for (int i = 0; i < packetSize; i++)
            {
                packet[i] = packets[(packetIndex * packetSize) + i];
            }

            return packet;
        }

        public byte[] Read()
        {
            if (CanRead)
                return buffer.Dequeue();
            else
                return null;
        }

        public byte[] Read(int packetCount)
        {
            if (!CanRead) return null;

            byte[] arr = new byte[packetCount * packetSize];

            for (int i = 0; i < packetCount; i++)
            {
                if (CanRead)
                {
                    byte[] packet = Read();
                    
                }
                else
                {
                    return arr;
                }
            }

            return arr;
        }

        public int PacketCount
        {
            get
            {
                return buffer.Count;
            }
        }

        public long Length
        {
            get
            {
                return buffer.Count * packetSize;
            }
        }
    }
}
