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

        public static int[,] CalcConfusionMatrix( MetricClassifier classifier, 
                                                  Vector[] test, 
                                                  string[] answers,
                                                  string[] classes )
        {
            var matrix = new int[classes.Length, classes.Length];
            var classIndexes = new Dictionary<string, int>( );
            for ( int i = 0; i < classes.Length; ++i )
            { classIndexes.Add( classes[i], i ); }

            var classified = classifier.Classify( test );

            for ( int i = 0; i < classified.Length; ++i )
            { 
                ++matrix[classIndexes[classified[i]], classIndexes[answers[i]]]; 
            }

            return matrix;
        }

        public static double[] CalcPrecision( int[,] confusionMatrix )
        {
            var result = new double[confusionMatrix.GetLength( 0 )];
            for ( int i = 0; i < result.Length; ++i )
            {
                double sum = 0.0;
                for ( int j = 0; j < confusionMatrix.GetLength( 1 ); ++j )
                { sum += confusionMatrix[i, j]; }

                result[i] = confusionMatrix[i, i] / sum; 
            }

            return result;
        }

        public static double[] CalcRecall( int[,] confusionMatrix )
        {
            var result = new double[confusionMatrix.GetLength( 1 )];
            for ( int i = 0; i < result.Length; ++i )
            {
                double sum = 0.0;
                for ( int j = 0; j < confusionMatrix.GetLength( 0 ); ++j )
                { sum += confusionMatrix[j, i]; }

                result[i] = confusionMatrix[i, i] / sum; 
            }

            return result;
        }

        public static void PrintMatrix<T>( T[,] matrix )
        {
            Console.WriteLine( "[" );
            for ( int i = 0; i < matrix.GetLength( 0 ); ++i )
            {
                Console.Write( "\t[" );
                if ( matrix.GetLength( 1 ) == 0 )
                {
                    Console.Write( "]" );
                    break;
                }

                Console.Write( $" {matrix[i, 0]}" );

                for ( int j = 1; j < matrix.GetLength( 1 ); ++j )
                {
                    Console.Write( $", {matrix[i, j]}" );
                }
                Console.WriteLine( " ]" );
            }
            Console.WriteLine( "]" );
        }

        public static void Main( )
        {
            Vector[] data, test;
            string[] classes, testAns;
            GetData( out data, out classes, out test, out testAns );
            
            var distinctClasses = classes.Concat( testAns ).Distinct( ).ToArray( );

            var classifiers = new List<MetricClassifier>
            {
                new KNearestClassifier( 1, data, classes ),
                new KNearestClassifier( 5, data, classes ),
                new KWeightedNearestClassifier( 10,
                                                KWeightedNearestClassifier.LinearWeight( 10 ), 
                                                data, 
                                                classes ),
                new ParzenWindowClassifier( ParzenWindowClassifier.GaussKernel,
                                            0.35,
                                            data,
                                            classes ),
                new AdaptiveParzenClassifier( ParzenWindowClassifier.EpanechnikovKernel,
                                              2,
                                              data,
                                              classes ),
                new PotentialFunctionClassifier( 0.01,
                                                 data,
                                                 classes,
                                                 data.Select( x => 0.35 ).ToArray( ) )
            };

            var evalutions = from classifier in classifiers
                             let confusionMatrix = CalcConfusionMatrix( classifier, test, testAns, distinctClasses )
                             select new
                             {
                                 ConfusionMatrix = confusionMatrix,
                                 Precisions = CalcPrecision( confusionMatrix ),
                                 Recalls = CalcRecall( confusionMatrix ),
                                 Classifier = classifier
                             };

            foreach ( var e in evalutions )
            {
                Console.WriteLine( $"--- {e.Classifier}:" );

                PrintMatrix( e.ConfusionMatrix );

                for ( int i = 0; i < distinctClasses.Length; ++i )
                {
                    Console.WriteLine( $"Class {distinctClasses[i]}:" );
                    Console.WriteLine( $"\tPrecision: {e.Precisions[i]}" );
                    Console.WriteLine( $"\tRecall: {e.Recalls[i]}" );
                }
            }
        }
    }
}
