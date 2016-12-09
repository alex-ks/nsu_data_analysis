using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Lab2
{
    public class LogisticRegression
    {
        private Vector coefficients;
        public double Threshold { get; set; }
        public double Eps { get; set; } = 1e-6;

        private double Sigmoid( double z ) => 1.0 / ( 1 + Math.Exp( -z ) );

        public bool[] Classify( Vector[] sample )
        {
            var answers = new bool[sample.Length];
            var tr = Threshold;
            for ( int i = 0; i < sample.Length; ++i )
            {
                answers[i] = Math.Sign( Sigmoid( sample[i] * coefficients ) - tr ) == 1;
            }
            return answers;
        }

        public void Fit( Vector[] train, bool[] answers )
        {
            double delta = 0, oldDelta = 0, eps = Eps;
            Vector c = coefficients;
            double alpha = 1;
            double norm = 1.0 / train.Length;
            do
            {
                oldDelta = delta;
                var cNext = c + norm * alpha * ( from i in Enumerable.Range( 0, train.Length )
                                                 select ( Sign( answers[i] ) - Sigmoid( c * train[i] ) ) * train[i] )
                                                 .Sum( );
                delta = Euclid.Dist( cNext, c );
                c = cNext;
            }
            while ( delta > eps && Math.Abs( delta - oldDelta ) > eps );
            coefficients = c;
        }

        int Sign( bool f ) => f ? 1 : 0;
    }
}
