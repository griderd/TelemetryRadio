using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;

namespace TelemetryRadio
{
    static class LineOfSight
    {
        static int[][] connections;

        public static void Solve()
        {
            connections = new int[FlightGlobals.Vessels.Count][];
            List<int> c = new List<int>();

            for (int i = 0; i < FlightGlobals.Vessels.Count; i++)
            {
                for (int j = 0; j < FlightGlobals.Vessels.Count; j++)
                {
                    if (i == j) j++;

                    Vessel a = FlightGlobals.Vessels[i];
                    Vessel b = FlightGlobals.Vessels[j];

                    Vector3d AtoB = a.GetWorldPos3D() + b.GetWorldPos3D();
                }
            }
        }
    }
}
