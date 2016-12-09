using System;

namespace Komissarov.Lab2
{
    public static class Euclid
    {
        public static double Norm( this Vector v )
        {
            return Math.Sqrt( v * v );
        }

        public static double Dist( Vector a, Vector b )
        {
            return ( a - b ).Norm( ); 
        }
    }
}