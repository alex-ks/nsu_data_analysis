using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace Lab1
{
    public class Program
    {
        public static void GetData( out Vector[] data, 
                                    out string[] classes, 
                                    out Vector[] test,
                                    out string[] testAns )
        {
            using ( var stream = new FileStream( "iris.csv", FileMode.Open ) )
            {
                using ( var csvReader = new CsvReader( new StreamReader( stream ), 
                                                       new CsvConfiguration { HasHeaderRecord = false } ) )
                {
                    var random = new Random( DateTime.Now.Millisecond );

                    var rows = csvReader
                        .GetRecords<IrisRecord>( )
                        .OrderBy( x => random.Next( 0, 150 ) )
                        .ToList( );

                    var sampleOfClass1 = rows.Where( row => row.Class == IrisRecord.Class1 );
                    var sampleOfClass2 = rows.Where( row => row.Class == IrisRecord.Class2 );
                    var sampleOfClass3 = rows.Where( row => row.Class == IrisRecord.Class3 ); 

                    var trainOfClass1 = sampleOfClass1.Take( 40 );
                    var testOfClass1 = sampleOfClass1.Reverse( ).Take( 10 );
                    
                    var trainOfClass2 = sampleOfClass2.Take( 40 );
                    var testOfClass2 = sampleOfClass2.Reverse( ).Take( 10 );
                    
                    var trainOfClass3 = sampleOfClass3.Take( 40 );
                    var testOfClass3 = sampleOfClass3.Reverse( ).Take( 10 );

                    test = ( from row in testOfClass1.Concat( testOfClass2 ).Concat( testOfClass3 )
                             select new Vector( row.SepalLength, 
                                                row.SepalWidth, 
                                                row.PetalLength, 
                                                row.PetalWidth ) ).ToArray( ); 

                    testAns = ( from row in testOfClass1.Concat( testOfClass2 ).Concat( testOfClass3 )
                                select row.Class ).ToArray( );

                    var train = trainOfClass1.Concat( trainOfClass2 ).Concat( trainOfClass3 );

                    data = ( from row in train
                             select new Vector( row.SepalLength, 
                                                row.SepalWidth,
                                                row.PetalLength,
                                                row.PetalWidth ) ).ToArray( );

                    classes = ( from row in train
                                select row.Class ).ToArray( );
                }
            }
        }

        public static void Main( )
        {
            Vector[] data, test;
            string[] classes, testAns;
            GetData( out data, out classes, out test, out testAns );
            
            //var classifier = new KNearestClassifier( 1, data, classes );
            var classifier = new KWeightedNearestClassifier( 10,
                                                             KWeightedNearestClassifier.LinearWeight( 10 ), 
                                                             data, 
                                                             classes );

            var ans = classifier.Classify( test );

            int errorCount = 0;
            for ( int i = 0; i < test.Length; ++i )
            {
                if ( ans[i] != testAns[i] )
                { ++errorCount; }
            }
            Console.WriteLine( errorCount );
        }
    }
}
