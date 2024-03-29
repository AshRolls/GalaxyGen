﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Framework
{
    public struct Vector2 : IEquatable<Vector2>
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

        public static bool operator ==(Vector2 lhs, Vector2 rhs) => lhs.Equals(rhs);

        public static bool operator !=(Vector2 lhs, Vector2 rhs) => !(lhs == rhs);

        [JsonIgnore]
        public Vector2 UnitVector
        {
            get { return this / Length; }
        }

        [JsonIgnore]
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

        public bool Equals(Vector2 p) => X == p.X && Y == p.Y;
        public override bool Equals(object obj) => obj is Vector2 other && this.Equals(other);
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    public class Long64Array : IEquatable<Long64Array>
    {
        public long[] Values = new long[64];

        public override bool Equals(object obj) => obj is Long64Array o && Equals(o);

        public bool Equals(Long64Array other)
        {
            for (int i = 0; i < 64; i++)
            {
                if (Values[i] != other.Values[i]) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            HashCode hash = new();
            for (int i = 0; i < 64; i++)
            {
                hash.Add(Values[i]);
            }
            return hash.ToHashCode();
        }
    }
}
