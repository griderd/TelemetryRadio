using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelemetryRadio.Protocols
{
    public abstract class PacketBuilder 
    {
        protected List<byte> body;

        public PacketBuilder()
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

        public void Add(char[] value)
        {
            Add(Encoding.UTF8.GetBytes(value));
        }

        public abstract IPacket ToPacket();
    }
}
