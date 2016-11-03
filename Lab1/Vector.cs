namespace Lab1
{
    public struct Vector
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public double T { get; }

        public Vector( double x, double y, double z, double t )
        {
            X = x;
            Y = y;
            Z = z;
            T = t;
        }
    }
}