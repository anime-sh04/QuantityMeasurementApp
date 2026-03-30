namespace QuantityMeasurementAppModelLayer.DTOs
{
    public enum LengthUnit
    {
        Inch, Foot, Yard, Centimeters
    }

    public enum WeightUnit
    {
        Gram, Kilogram,Pound
    }

    public enum VolumeUnit
    {
        Milliliter, Liter, Gallon
    }

    public enum TemperatureUnit
    {
        Celsius, Fahrenheit, Kelvin
    }

    public class QuantityDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string MeasurementType { get; set; } = string.Empty; // "Length","Weight","Volume","Temperature"

        public QuantityDTO() { }

        public QuantityDTO(double value, string unit, string measurementType)
        {
            Value = value;
            Unit = unit;
            MeasurementType = measurementType;
        }
    }
}
