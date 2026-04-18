namespace QuantityMeasurementAppModelLayer.Models
{
    public class QuantityModel
    {
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;

        public QuantityModel(double value, string unit)
        {
            Value = value;
            Unit = unit;
        }

        public override string ToString() => $"{Value} {Unit}";
    }
}
