using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace Lab1
{
    public class Program
    {
        public static void GetData( out List<Vector> data, out List<string> classes )
        {
            using ( var stream = new FileStream( "iris.csv", FileMode.Open ) )
            {
                using ( var csvReader = new CsvReader( new StreamReader( stream ) ) )
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
                                                row.PetalWidth ) ).ToList( );

                    classes = ( from row in rows
                                select row.Class ).ToList( );
                }
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
