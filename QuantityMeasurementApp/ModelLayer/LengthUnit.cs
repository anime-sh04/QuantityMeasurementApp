namespace QuantityMeasurementApp.ModelLayer;

public enum LengthUnit
{
    Feet,
    Inch,
    Yard,
    Centimeter
}

public static class LengthUnitExtension
{
    // Convert unit value → base unit (Feet)
    public static double ConvertToBaseUnit(this LengthUnit unit, double value)
    {
        return unit switch
        {
            LengthUnit.Feet => value,
            LengthUnit.Inch => value / 12.0,
            LengthUnit.Yard => value * 3.0,
            LengthUnit.Centimeter => value * 0.0328084,
            _ => throw new ArgumentException("Unsupported unit")
        };
    }

    // Convert base unit (Feet) → target unit
    public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
    {
        return unit switch
        {
            LengthUnit.Feet => baseValue,
            LengthUnit.Inch => baseValue * 12.0,
            LengthUnit.Yard => baseValue / 3.0,
            LengthUnit.Centimeter => baseValue / 0.0328084,
            _ => throw new ArgumentException("Unsupported unit")
        };
    }
}