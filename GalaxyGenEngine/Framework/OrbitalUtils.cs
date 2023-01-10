using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Framework
{    
    public static class OrbitalUtils
    {        
        public static PointD CalcPositionFromTick(Int64 tick, Double orbitalTicks, Double orbitalKm)
        {
            double tickMod = tick % orbitalTicks;
            double slice = 2 * Math.PI / orbitalTicks;
            double angle = slice * tickMod;
            PointD pt;
            pt.X = orbitalKm * Math.Cos(angle);
            pt.Y = orbitalKm * Math.Sin(angle);
            return pt;
        }

        public static PointD CalcCenteredPositionFromTick(Int64 tick, Double orbitalTicks, Double orbitalKm, PointD center)
        {
            double tickMod = tick % orbitalTicks;
            double slice = 2 * Math.PI / orbitalTicks;
            double angle = slice * tickMod;
            PointD pt;
            pt.X = center.X + orbitalKm * Math.Cos(angle);
            pt.Y = center.Y + orbitalKm * Math.Sin(angle);
            return pt;
        }
    }
}
