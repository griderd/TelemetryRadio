using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteTech;

namespace TelemetryRadio
{
    public class RadioTracker
    {
        public static List<Guid> receivers = new List<Guid>();

        public static void AddReceiver(Guid receiver)
        {
            if (!Contains(receiver))
                receivers.Add(receiver);
        }

        public static bool Contains(Guid receiver)
        {
            return receivers.Contains(receiver);
        }
    }
}
