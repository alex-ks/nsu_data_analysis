using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    public abstract class MetricClassifier
    {
        protected Vector[] train;
        protected string[] answers;

        protected abstract double CalcNeighbourWeight( Vector u, Vector neighbour, int neighbourIndex );

        struct Record
        {
            public Vector Values { get; set; }
            public string Class { get; set; }
        }

        IEnumerable<Record> GetSampleUnion( )
        {
            for ( int i = 0; i < train.Length; ++i )
            {
                yield return new Record { Values = train[i], Class = answers[i] };
            }
        }

        public MetricClassifier( Vector[] train, string[] answers )
        {
            if ( train.Length != answers.Length )
            {
                throw new ArgumentException( "Answers count must be equal to train sample size." );
            }

            this.train = train;
            this.answers = answers;
        }

        public string[] Classify( Vector[] sample )
        {
            var result = new List<string>( );

            var classes = answers.Distinct( ).ToList( );

            var classDists = new Dictionary<string, double>( );

            foreach ( var u in sample )
            {
                classDists.Clear( );

                foreach ( var c in classes )
                { classDists.Add( c, 0 ); }  

                int i = 0;

                foreach ( var v in GetSampleUnion( ).OrderBy( v => Euclid.Dist( u, v.Values ) ) )
                {
                    classDists[v.Class] += CalcNeighbourWeight( u, v.Values, i++ );
                }

                // To make class with max weight first
                result.Add( classDists.OrderByDescending( pair => pair.Value ).First( ).Key );
            }

            return result.ToArray( );
        }

        public string Classify( Vector u )
        {
            var classes = answers.Distinct( ).ToList( );

            var classDists = new Dictionary<string, double>( );

            foreach ( var c in classes )
            { classDists.Add( c, 0 ); }  

            int i = 0;

            foreach ( var v in GetSampleUnion( ).OrderBy( v => Euclid.Dist( u, v.Values ) ) )
            {
                classDists[v.Class] += CalcNeighbourWeight( u, v.Values, i++ );
            }

            // To make class with max weight first
            return classDists.OrderByDescending( pair => pair.Value ).First( ).Key;
        }
    }
}
