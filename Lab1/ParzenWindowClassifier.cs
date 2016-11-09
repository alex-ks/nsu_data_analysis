using System;

namespace Lab1
{
    public class ParzenWindowClassifier : MetricClassifier
    {
        private Func<double, double> kernelFunc;
        private double h;

        public ParzenWindowClassifier( Func<double, double> kernel,
                                       double h,
                                       Vector[] train, 
                                       string[] answers ) : base( train, answers )
        {
            kernelFunc = kernel;
            this.h = h;
        }

        protected override double CalcNeighbourWeight( Vector u, Vector neighbour, int neighbourIndex )
        {
            return kernelFunc( Euclid.Dist( u, neighbour ) / h );
        }

        public override string ToString( )
        {
            return nameof( ParzenWindowClassifier );
        }

        public static readonly Func<double, double> GaussKernel = 
            x => 1.0 / Math.Sqrt( 2 * Math.PI ) * Math.Exp( -0.5 * x * x );

        public static readonly Func<double, double> EpanechnikovKernel = 
            x => Math.Abs( x ) <= 1 ? 3.0 / 4.0 * ( 1 - x * x ) : 0;

        public static readonly Func<double, double> TriangularKernel = 
            x => Math.Abs( x ) <= 1 ? ( 1 - Math.Abs( x ) ) : 0;

        public static readonly Func<double, double> UniformKernel =
            x => Math.Abs( x ) <= 1 ? 0.5 : 0;
    }
}