using System;
using System.Linq;

namespace Lab1
{
    public class AdaptiveParzenClassifier : MetricClassifier
    {
        private int hSource;
        private Func<double, double> kernelFunc;

        // Use kernels from ParzenWindowClassifier   
        public AdaptiveParzenClassifier( Func<double, double> kernel, 
                                         int hSource,
                                         Vector[] train, 
                                         string[] answers ) : base( train, answers )
        {
            this.hSource = hSource;
            kernelFunc = kernel;
        }

        public override string ToString( )
        {
            return nameof( AdaptiveParzenClassifier );
        }

        // It's assumed that there is no zero-vector in sample
        private Vector cachedU;
        private Vector sourceVec;

        protected override double CalcNeighbourWeight( Vector u, Vector neighbour, int neighbourIndex )
        {
            if ( u != cachedU )
            {
                sourceVec = train.OrderBy( x => Euclid.Dist( u, x ) ).ElementAt( hSource );
                cachedU = u; 
            }

            double h = Euclid.Dist( u, sourceVec );
            return kernelFunc( Euclid.Dist( u, neighbour ) / h );
        }
    }
}