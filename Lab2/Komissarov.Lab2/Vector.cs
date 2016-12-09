using System.Collections.Generic;

namespace Komissarov.Lab2
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

        public static Vector operator +( Vector a, Vector b )
        {
            return new Vector( a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.T + b.T );
        }

        public static Vector operator -( Vector a, Vector b )
        {
            return new Vector( a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.T - b.T );
        }

        public static double operator *( Vector a, Vector b )
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.T * b.T;
        }

        public static Vector operator *( double scalar, Vector v )
        {
            return new Vector( scalar * v.X, scalar * v.Y, scalar * v.Z, scalar * v.T );
        }

        public static bool operator ==( Vector a, Vector b )
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.T == b.T;
        }

        public static bool operator !=( Vector a, Vector b )
        {
            return !( a == b );
        }

        // override object.Equals
        public override bool Equals( object obj )
        {
            return obj is Vector && ( ( Vector )obj == this );
        }

        public override int GetHashCode( )
        {
            return X.GetHashCode( ) ^ Y.GetHashCode( ) ^ Z.GetHashCode( ) ^ T.GetHashCode( );
        }
    }

    public static class EnumerableVectorHelper
    {
        public static Vector Sum( this IEnumerable<Vector> vectors )
        {
            var result = new Vector( );
            foreach ( var v in vectors )
            { result += v; }
            return result;
        }
    }
}
