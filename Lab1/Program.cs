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
        public static void GetData( out Vector[] data, out string[] classes )
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

                    data = ( from row in rows
                             select new Vector( row.SepalLength, 
                                                row.SepalWidth,
                                                row.PetalLength,
                                                row.PetalWidth ) ).ToArray( );

                    classes = ( from row in rows
                                select row.Class ).ToArray( );
                }
            }
        }

        public static void Main( )
        {
            Vector[] data;
            string[] classes;
            GetData( out data, out classes );

            Console.WriteLine( data.Length );
        }
    }
}
