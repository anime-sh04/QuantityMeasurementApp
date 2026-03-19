namespace QuantityMeasurementAppModelLayer.Models
{
    public class QuantityMeasurementEntity
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}