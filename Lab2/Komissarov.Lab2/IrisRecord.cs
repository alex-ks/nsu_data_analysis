namespace Komissarov.Lab2
{
    public class IrisRecord
    {
        public double SepalLength { get; set; }
        public double SepalWidth { get; set; }
        public double PetalLength { get; set; }
        public double PetalWidth { get; set; }

        public string Class { get; set; }

        public const string Class1 = "Iris-setosa";
        public const string Class2 = "Iris-versicolor";
        public const string Class3 = "Iris-virginica";
    }
}
