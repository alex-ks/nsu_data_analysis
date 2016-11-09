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

        protected override double CalcNeighbourWeight( Vector u, Vector neighbour, int neighbourIndex )
        {
            double h = Euclid.Dist( u, train[hSource] );
            return kernelFunc( Euclid.Dist( u, neighbour ) / h );
        }
    }
}