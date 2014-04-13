using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelemetryRadio.Protocols.UDP;
using GSLib.Extensions;

namespace RadioTester
{
    class Program
    {
        static void Main(string[] args)
        {
            UDPPacketBuilder a = new UDPPacketBuilder(1);
            a.Add("Hello World!".Length);
            a.Add("Hello World!");
            UDPPacketBuilder b = new UDPPacketBuilder(1);
            b.Add("Again, hello world!".Length);
            b.Add("Again, hello world!");

            List<byte> stream = new List<byte>();
            stream.AddRange(a.ToPacket().ToByteArray());
            stream.AddRange(Encoding.UTF8.GetBytes(UDPPacket.PSEUDOHEADER));
            stream.AddRange(b.ToPacket().ToByteArray());
            byte[] overflow;

            //Console.WriteLine(b1.ArrayToString("", "g", true));
            //Console.WriteLine();
            //Console.WriteLine(b2.ArrayToString("", "g", true));
            //Console.WriteLine();

            Console.WriteLine("Stream: " + stream.ToArray().ArrayToString("", "g", true));
            Console.WriteLine();
            Decode(stream.ToArray(), out overflow);
            Console.WriteLine();
            Console.WriteLine("Overflow: " + overflow.ArrayToString("", "g", true));
            Decode(overflow, out overflow);
            Console.WriteLine();
            Console.WriteLine("Overflow: " + overflow.ArrayToString("", "g", true));

            Console.ReadLine();
        }

        static void Decode(byte[] b, out byte[] overflow)
        {
            UDPPacketDecoder decoder = new UDPPacketDecoder();
            decoder.AddBytes(b, out overflow);

            int len;
            string s;

            if (!decoder.CanRead)
                Console.WriteLine("STILL IN HEADER!!!");
            else if (!decoder.EndOfPacket)
                Console.WriteLine("NOT AT END OF PACKET!!!");
            else if (!decoder.PacketIsValid)
                Console.WriteLine("PACKET IS NOT VALID!!!");
            else
            {
                decoder.TryReadInt32(out len);
                decoder.TryReadString(len, out s);
                Console.WriteLine(s);
            }
        }
    }
}
