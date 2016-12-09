using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Lab2
{
    public class SampleLoader
    {
        public static List<IrisRecord> LoadRecords( string fileName )
        {
            using ( var stream = new FileStream( fileName, FileMode.Open ) )
            {
                using ( var csvReader = new CsvReader( new StreamReader( stream ),
                                                       new CsvConfiguration { HasHeaderRecord = false } ) )
                {
                    return csvReader.GetRecords<IrisRecord>( )
                                    .ToList( );
                }
            }
        }

        public static void ExtractSample( List<IrisRecord> records,
                                          out Vector[] data,
                                          out string[] classes,
                                          out Vector[] test,
                                          out string[] testAns )
        {
            var random = new Random( DateTime.Now.Millisecond );

            var rows = records.OrderBy( x => random.Next( records.Count ) );

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
