using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;
using RemoteTech;

namespace TelemetryRadio
{
    public class Air
    {   
        static List<DataPacket>[] channels = new List<DataPacket>[ChannelCount];

        public static int ChannelCount { get { return 8; } }
        public static int PacketSize { get { return 4; } }

        public static void WriteToChannel(int channel, byte[] data, Guid source)
        {
            if ((channel < 0) | (channel >= ChannelCount)) throw new ArgumentOutOfRangeException("Channel index out of range.");
            if (data.Length != PacketSize) throw new ArgumentOutOfRangeException("Data must contain PacketSize number of bytes.");

            DataPacket packet = new DataPacket(data, Planetarium.GetUniversalTime(), source);
            channels[channel].Add(packet);
        }
    }

    public struct DataPacket
    {
        public byte[] data;
        public double transmitTime;
        public Guid source;

        public DataPacket(byte[] data, double transmitTime, Guid source)
        {
            this.data = data;
            this.transmitTime = transmitTime;
            this.source = source;
        }
    }
}
