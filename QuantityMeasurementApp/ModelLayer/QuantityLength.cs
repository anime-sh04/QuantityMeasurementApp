namespace QuantityMeasurementApp.ModelLayer;

// UC3/UC4/UC5: Generic Quantity class for length measurements
// Supports Feet, Inches, Yards and Centimeters with DRY conversion logic
public class QuantityLength
{
    private readonly double value;
    private readonly LengthUnit unit;

    // Conversion constant
    private const double INCH_TO_FEET = 1.0 / 12.0;

    public QuantityLength(double value, LengthUnit unit)
    {
        this.value = value;
        this.unit = unit;
    }

    // Converts this object's value to base unit (Feet)
    private double ToBaseUnitInFeet()
    {
        return unit switch
        {
            LengthUnit.Feet => value,

            // 1 inch = 1/12 feet
            LengthUnit.Inch => value * INCH_TO_FEET,

            // 1 yard = 3 feet
            LengthUnit.Yard => value * 3.0,

            // 1 cm = 0.0328084 feet
            LengthUnit.Centimeter => value * 0.0328084,

            _ => throw new ArgumentException("Unsupported unit type")
        };
    }

    // =========================
    // UC5 STATIC CONVERSION API
    // =========================
    public static double Convert(double value, LengthUnit source, LengthUnit target)
    {
        // Validate numeric value
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Value must be a finite number");

        // If units are the same
        if (source == target)
            return value;

        // Convert source → feet
        double valueInFeet = ConvertToFeet(value, source);

        // Convert feet → target
        return ConvertFromFeet(valueInFeet, target);
    }

    // Helper: Convert any unit to feet
    private static double ConvertToFeet(double value, LengthUnit unit)
    {
        return unit switch
        {
            LengthUnit.Feet => value,
            LengthUnit.Inch => value * INCH_TO_FEET,
            LengthUnit.Yard => value * 3.0,
            LengthUnit.Centimeter => value * 0.0328084,
            _ => throw new ArgumentException("Unsupported unit")
        };
    }

    // Helper: Convert feet to target unit
    private static double ConvertFromFeet(double feetValue, LengthUnit target)
    {
        return target switch
        {
            LengthUnit.Feet => feetValue,
            LengthUnit.Inch => feetValue / INCH_TO_FEET,
            LengthUnit.Yard => feetValue / 3.0,
            LengthUnit.Centimeter => feetValue / 0.0328084,
            _ => throw new ArgumentException("Unsupported unit")
        };
    }

    // =========================
    // UC5 INSTANCE CONVERSION
    // =========================
    public double ConvertTo(LengthUnit targetUnit)
    {
        return Convert(this.value, this.unit, targetUnit);
    }

    // =========================
    // EQUALITY CHECK
    // =========================
    public override bool Equals(object? obj)
    {
        if (this == obj)
            return true;

        if (obj == null || obj.GetType() != typeof(QuantityLength))
            return false;

        QuantityLength other = (QuantityLength)obj;

        double thisInFeet = this.ToBaseUnitInFeet();
        double otherInFeet = other.ToBaseUnitInFeet();

        // Floating point tolerance
        const double EPSILON = 1e-6;
        return Math.Abs(thisInFeet - otherInFeet) < EPSILON;
    }

    // Required when Equals() is overridden
    public override int GetHashCode()
    {
        return ToBaseUnitInFeet().GetHashCode();
    }

    // Useful for debugging/logging
    public override string ToString()
    {
        return $"{value} {unit}";
    }
}