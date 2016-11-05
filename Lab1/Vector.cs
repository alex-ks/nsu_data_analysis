using System;

namespace Lab1
{
    public struct Vector
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double T { get; }

        public Vector( double x, double y, double z, double t )
        {
            X = x;
            Y = y;
            Z = z;
            T = t;
        }

        public static Vector operator -( Vector a, Vector b )
        {
            return new Vector( a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.T - b.T );
        }

        public static double operator *( Vector a, Vector b )
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.T * b.T;
        }
    }
}