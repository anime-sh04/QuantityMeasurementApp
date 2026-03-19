namespace QuantityMeasurementAppModelLayer.Models
{
    public class QuantityModel
    {
        public double BaseValue { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}