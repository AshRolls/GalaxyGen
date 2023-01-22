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
        public static Vector2 CalcPositionFromTick(UInt64 tick, Double orbitalTicks, Double orbitalKm)
        {
            double tickMod = tick % orbitalTicks;
            double slice = 2 * Math.PI / orbitalTicks;
            double angle = slice * tickMod;
            return new Vector2(orbitalKm * Math.Cos(angle), orbitalKm * Math.Sin(angle));            
        }

        public static Vector2 CalcCenteredPositionFromTick(UInt64 tick, Double orbitalTicks, Double orbitalKm, Vector2 center)
        {
            double tickMod = tick % orbitalTicks;
            double slice = 2 * Math.PI / orbitalTicks;
            double angle = slice * tickMod;            
            return new Vector2(center.X + orbitalKm * Math.Cos(angle), center.Y + orbitalKm * Math.Sin(angle));
        }
    }
}
