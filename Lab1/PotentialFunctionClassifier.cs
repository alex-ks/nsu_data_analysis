using System;
using System.Linq;

namespace Lab1
{
    public class PotentialFunctionClassifier : MetricClassifier
    {
        private double[] charges;
        private double[] widths;
        private double divideConst;

        public PotentialFunctionClassifier( double learningPrecision,
                                            Vector[] train, 
                                            string[] answers,
                                            double[] widths,
                                            double divideConst = 1.0 ) : base( train, answers )
        {
            if ( widths.Length != train.Length )
            { throw new ArgumentException( "Width count must be equal to train sample size" ); }
            this.widths = widths;
            this.divideConst = divideConst;

            charges = new double[train.Length];

            int errorCount;
            int learningErrorLimit = ( int )( learningPrecision * train.Length );

            do
            {
                errorCount = 0;
                for ( int i = 0; i < train.Length; ++i )
                {
                    if ( Classify( train[i] ) != answers[i] )
                    {
                        ++charges[i];
                        ++errorCount;
                    }
                }
            }
            while ( errorCount > learningErrorLimit );
        }

        private Vector cachedU;
        private int[] indexOrder;

        private double PotentialFunction( double r ) => 1.0 / ( r + divideConst );

        protected override double CalcNeighbourWeight( Vector u, Vector neighbour, int neighbourIndex )
        {
            if ( u != cachedU )
            {
                indexOrder = ( from quant in train.Select( ( x, i ) => new { Values = x, Index = i } )
                               orderby Euclid.Dist( u, quant.Values )
                               select quant.Index ).ToArray( );
                cachedU = u;
            }

            var dist = Euclid.Dist( u, neighbour ) / widths[indexOrder[neighbourIndex]];

            return charges[indexOrder[neighbourIndex]] * PotentialFunction( dist );
        }

        public override string ToString( )
        {
            return nameof( PotentialFunctionClassifier );
        }
    }
}