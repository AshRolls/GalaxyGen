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
        // http://stackoverflow.com/a/12550458/771034
        // Length function is slow (sqrt!) Use approx 
        public static PointD GetNewPointForShip(Double distance, Double aX, Double aY, Double bX, Double bY)
        {
            Vector2 aV = new Vector2(aX, aY);
            Vector2 bV = new Vector2(bX, bY);
            Vector2 abV = aV - bV;
            double length = abV.Length;
            if (length < distance) return new PointD(bX, bY); // don't overshoot
            abV /= length;
            abV *= distance;
            return new PointD(abV.X + aX, abV.Y + aY);
        }

        // Returns the length of the hypotenuse rounded to an integer, using
        // Pythagoras' Theorem for right angle triangles: The length of the
        // hypotenuse equals the sum of the square of the other two sides.
        // Ergo: h = Sqrt(a*a + b*b)
        private static double LengthOfHypotenuse(Double aX, Double aY, Double bX, Double bY)
        {
            double aSq = Math.Pow(aX - bX, 2); // horizontal length squared
            double bSq = Math.Pow(aY - bY, 2); // vertical length  squared
            return Math.Sqrt(aSq + bSq); // length of the hypotenuse
        }
    }
}
