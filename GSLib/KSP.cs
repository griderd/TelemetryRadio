using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP;

namespace GSLib
{
    public class KSPHelpers
    {
        public static CelestialBody GetCelestialBody(string name)
        {
            foreach (CelestialBody b in FlightGlobals.Bodies)
            {
                if (b.name == name)
                    return b;
            }
            return null;
        }

        public static double DistanceToSurface(CelestialBody body, Vessel vessel)
        {
            return Vector3d.Distance(body.GetOrbit().getTruePositionAtUT(Planetarium.GetUniversalTime()), vessel.GetWorldPos3D()) - body.Radius;
        }

        public static ITargetable GetCurrentTarget()
        {
            return FlightGlobals.fetch.VesselTarget;
        }

        public static double GetDistanceToOrbit(Orbit o, Vessel vessel)
        {
            return Vector3d.Distance(o.getTruePositionAtUT(Planetarium.GetUniversalTime()), vessel.GetWorldPos3D());
        }
    }
}
