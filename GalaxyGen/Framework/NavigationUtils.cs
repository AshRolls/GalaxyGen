using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Framework
{    
    public static class NavigationUtils
    {        
        public static PointD GetNewPointForShip(Double shipSpeedKmHPerTick, Double curX, Double curY, Double destX, Double destY)
        {
            PointD newPoint;
            Double vX = destX - curX;
            Double vY = destY - curX;
            Double length = Math.Sqrt((vX * vX) + (vY * vY));
            if (length < shipSpeedKmHPerTick)
            {
                newPoint.X = destX;
                newPoint.Y = destY;
            }
            else
            {
                vX /= length;
                vY /= length;
                newPoint.X = (curX + vX) * (length + shipSpeedKmHPerTick);
                newPoint.Y = (curY + vY) * (length + shipSpeedKmHPerTick);
            }
            
            return new PointD();
        }
    }
}
