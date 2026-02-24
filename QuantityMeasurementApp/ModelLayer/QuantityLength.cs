
namespace QuantityMeasurementApp;

// UC3: Generic Quantity class to eliminate code duplication
// Handles multiple units like Feet and Inches using a single class
public class QuantityLength
{
    private readonly double value;
    private readonly LengthUnit unit;

    // Conversion factor: base unit = Feet
    private const double INCH_TO_FEET = 1.0 / 12.0;
    public QuantityLength(double value, LengthUnit unit)
    {
        this.value = value;
        this.unit = unit;
    }

    // Convert any unit to base unit (Feet)
    // Abstraction: hides conversion logic from client code
    private double ToBaseUnitInFeet()
    {
        return unit switch
        {
            LengthUnit.Feet => value,
    
            // 1 inch = 1/12 feet
            LengthUnit.Inch => value / 12.0,
    
            // 1 yard = 3 feet
            LengthUnit.Yard => value * 3.0,
    
            // 1 cm = 0.393701 inches → inches to feet → /12
            // OR directly: 1 cm = 0.0328084 feet
            LengthUnit.Centimeter => value * 0.0328084,
    
            _ => throw new ArgumentException("Unsupported unit type")
        };
    }

    // Override Equals for cross-unit value-based comparison
    public override bool Equals(object? obj)
    {
        // Reflexive property
        if (this == obj)
            return true;

        // Null safety + Type safety
        if (obj == null || obj.GetType() != typeof(QuantityLength))
            return false;

        QuantityLength other = (QuantityLength)obj;

        // Convert both values to common base unit (Feet)
        double thisInFeet = this.ToBaseUnitInFeet();
        double otherInFeet = other.ToBaseUnitInFeet();

        // Floating-point safe comparison
        return thisInFeet.CompareTo(otherInFeet) == 0;
    }

    // Required for Equality Contract
    public override int GetHashCode()
    {
        return ToBaseUnitInFeet().GetHashCode();
    }
}