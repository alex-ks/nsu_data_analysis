using System;

namespace Lab1
{
    public enum WeightHeuristics
    {
        Linear, Exponential, Distance
    }

    public class KWeightedNearestClassifier : MetricClassifier
    {
        private Func<Vector, Vector, int, double> weightFunc;
        private int k;

        public KWeightedNearestClassifier( int k, 
                                           Func<Vector, Vector, int, double> weightFunc,
                                           Vector[] train, 
                                           string[] answers ) : base( train, answers )
        {
            this.weightFunc = weightFunc;
            this.k = k;
        }

        protected override double CalcNeighbourWeight( Vector u, 
                                                       Vector neighbour, 
                                                       int neighbourIndex )
        {
            return neighbourIndex >= k ? 0 : weightFunc( u, neighbour, neighbourIndex );
        }

        public static Func<Vector, Vector, int, double> LinearWeight( int k )
        {
            return ( u, v, i ) => ( k - i ) / ( double )k;
        }

        public static Func<Vector, Vector, int, double> ExponentialWeight( double q )
        {
            return ( u, v, i ) => Math.Pow( q, i );
        }

        public static Func<Vector, Vector, int, double> DistanceWeight( )
        {
            // Because we maximize weight of class, nearest object must have the biggest weight 
            return ( u, v, i ) => 1.0 / Euclid.Dist( u, v );
        }

        public override string ToString( )
        {
            return nameof( KWeightedNearestClassifier );
        }
    }
}