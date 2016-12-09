using Caliburn.Micro;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komissarov.Lab2.ViewModels
{
    public class RocViewModel : PropertyChangedBase
    {
        public string[] AvailableClasses { get; } =
        {
            IrisRecord.Class1,
            IrisRecord.Class2,
            IrisRecord.Class3
        };

        private List<IrisRecord> records;
        private Vector[] test;
        private string[] testAns;
        private Vector[] train;
        private string[] trainAns;
        private LogisticRegression logistic;

        bool isAimClass( string className ) => CurrentClass == className;

        public RocViewModel( )
        {
            Model = new PlotModel { Title = "Wait, please..." };
            logistic = new LogisticRegression( );
            Task.Factory.StartNew( ( ) =>
            {
                records = SampleLoader.LoadRecords( "iris.csv" );
                SampleLoader.ExtractSample( records, out train, out trainAns, out test, out testAns );
            } ).ContinueWith( async ( t ) =>
            {
                await DrawRoc( );
            } );
        }

        public async void ReshuffleSample( )
        {
            CanInteract = false;
            SampleLoader.ExtractSample( records, out train, out trainAns, out test, out testAns );
            await DrawRoc( );
            CanInteract = true;
        }

        private async Task DrawRoc( )
        {
            CanInteract = false;

            Status = "Training classifier...";

            await Task.Factory.StartNew( ( ) => logistic.Fit( train,
                                                              trainAns.Select( x => isAimClass( x ) )
                                                                      .ToArray( ) ) );

            var series = new LineSeries( ) { Smooth = false, LineStyle = LineStyle.Solid };

            Status = "Iterating over thresholds...";

            for ( double tr = -10; tr <= 10; tr += 0.005 )
            {
                logistic.Threshold = tr;
                var ans = await Task.Factory.StartNew( ( ) => logistic.Classify( test ) );
                series.Points.Add( new DataPoint( CalcFPR( ans ), CalcTPR( ans ) ) );
            }

            var newModel = new PlotModel
            {
                Title = $"ROC for class {CurrentClass}",
            };

            newModel.Series.Add( series );
            newModel.Series.Add( new FunctionSeries( x => x, 0, 1, 0.01 ) );

            Model = newModel;
            NotifyOfPropertyChange( ( ) => Model );

            var plot = series.Points.Distinct( ).OrderBy( x => x.X * 10 + x.Y ).ToList( );

            Status = $"Area under curve: {CalcArea( plot )}";
            CanInteract = true;
        }

        private double CalcFPR( bool[] ans )
        {
            var count = Enumerable.Range( 0, ans.Length )
                                  .Where( i => ans[i] && !isAimClass( testAns[i] ) ).Count( );
            var denom = Enumerable.Range( 0, ans.Length )
                                  .Where( i => !isAimClass( testAns[i] ) ).Count( );

            return count / ( double )denom;
        }

        private double CalcTPR( bool[] ans )
        {
            var count = Enumerable.Range( 0, ans.Length )
                                  .Where( i => ans[i] && isAimClass( testAns[i] ) ).Count( );
            var denom = Enumerable.Range( 0, ans.Length )
                                  .Where( i => isAimClass( testAns[i] ) ).Count( );

            return count / ( double )denom;
        }

        private double CalcArea( List<DataPoint> points )
        {
            return ( from i in Enumerable.Range( 1, points.Count - 1 )
                     select 0.5 * ( points[i].Y + points[i - 1].Y ) * ( points[i].X - points[i - 1].X ) )
                     .Sum( );
        }

        private PlotModel model;
        public PlotModel Model
        {
            get { return model; }
            set
            {
                if ( Equals( model, value ) )
                { return; }
                model = value;
                NotifyOfPropertyChange( ( ) => Model );
            }
        }

        private string currentClass = IrisRecord.Class1;
        public string CurrentClass
        {
            get { return currentClass; }

            set
            {
                if ( value == currentClass )
                { return; }
                currentClass = value;
                NotifyOfPropertyChange( ( ) => CurrentClass );
                // Return from setter before the call is completed is intended behaviour
                DrawRoc( );
            }
        }

        private bool canInteract;

        public bool CanInteract
        {
            get { return canInteract; }
            set
            {
                if ( value == canInteract )
                { return; }
                canInteract = value;
                NotifyOfPropertyChange( ( ) => CanInteract );
            }
        }

        private string status;
        public string Status
        {
            get { return status; }

            set
            {
                if ( value == status )
                { return; }
                status = value;
                NotifyOfPropertyChange( ( ) => status );
            }
        }
    }
}
