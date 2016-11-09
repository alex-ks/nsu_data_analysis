using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    public class PotentialFunctionClassifier : MetricClassifier
    {
        // struct Quant
        // {
        //     public Vector Values { get; }
        //     public double Width { get; }
        //     public double Charge { get; }
        //     public int Index { get; }

        //     public Quant( Vector values, double width, double charge, int index )
        //     {
        //         Values = values;
        //         Width = width;
        //         Charge = charge;
        //         Index = index;
        //     }
        // }

        private double[] charges;
        private double[] widths;
        private double divideConst;

        // private IEnumerable<Quant> GetQuants( )
        // {
        //     for ( int i = 0; i < train.Length; ++i )
        //     { yield return new Quant( train[i], widths[i], charges[i], i ); }
        // }

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
    }
}