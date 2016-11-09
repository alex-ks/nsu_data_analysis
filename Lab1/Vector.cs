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
}
