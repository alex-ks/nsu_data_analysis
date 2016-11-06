namespace Lab1
{
    public class KNearestClassifier : MetricClassifier
    {
        private int k;

        public KNearestClassifier( int k, Vector[] train, string[] answers ) : base( train, answers )
        {
            this.k = k;
        }

        protected override double CalcNeighbourWeight( Vector u, 
                                                       Vector neighbour, 
                                                       int neighbourIndex )
        {
            return neighbourIndex >= k ? 0 : 1;
        }
    }
}
