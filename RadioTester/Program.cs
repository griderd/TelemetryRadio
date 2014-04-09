using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelemetryRadio.Protocols;
using GSLib.Extensions;

namespace RadioTester
{
    class Program
    {
        static void Main(string[] args)
        {
            UDPPacketBuilder a = new UDPPacketBuilder(1);
            a.Add("Hello World!");
            UDPPacketBuilder b = new UDPPacketBuilder(1);
            b.Add("Again, hello world!");

            //UDPPacket p1 = a.ToPacket();
            //UDPPacket p2 = b.ToPacket();

            Console.WriteLine(a.ToPacket().ToByteArray().ArrayToString("", "g", true));
            Console.WriteLine();
            Console.WriteLine(b.ToPacket().ToByteArray().ArrayToString("", "g", true));

            Console.ReadLine();
        }
    }
}
