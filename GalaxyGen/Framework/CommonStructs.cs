using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Framework
{
    public struct PointD
    {
        public PointD(Double x, Double y)
        {
            X = x;
            Y = y;
        }

        public Double X;
        public Double Y;
    }

    public struct Vector2
    {
        public readonly double X;
        public readonly double Y;
        public Vector2(Point p) : this(p.X, p.Y)
        {
        }

        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(b.X - a.X, b.Y - a.Y);
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(b.X + a.X, b.Y + a.Y);
        }
        public static Vector2 operator *(Vector2 a, double d)
        {
            return new Vector2(a.X * d, a.Y * d);
        }
        public static Vector2 operator /(Vector2 a, double d)
        {
            return new Vector2(a.X / d, a.Y / d);
        }

        public static implicit operator Point(Vector2 a)
        {
            return new Point((int)a.X, (int)a.Y);
        }

        public Vector2 UnitVector
        {
            get { return this / Length; }
        }

        public double Length
        {
            get
            {
                double aSq = Math.Pow(X, 2);
                double bSq = Math.Pow(Y, 2);
                return Math.Sqrt(aSq + bSq);
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", X, Y);
        }
    }
}
