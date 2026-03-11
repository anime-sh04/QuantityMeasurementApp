namespace QuantityMeasurementApp.ModelLayer;

public enum LengthUnit
{
    Feet,
    Inch,
    Yard,
    Centimeter
}

public static class LengthUnitExtensions
{
    public static double GetConversionFactor(this LengthUnit unit)
    {
        return unit switch
        {
            LengthUnit.Feet => 1.0,
            LengthUnit.Inch => 1.0 / 12.0,
            LengthUnit.Yard => 3.0,
            LengthUnit.Centimeter => 0.0328084,
            _ => throw new ArgumentException("Unsupported unit")
        };
    }

    public static double ConvertToBaseUnit(this LengthUnit unit, double value)
    {
        return value * unit.GetConversionFactor();
    }

    public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
    {
        return baseValue / unit.GetConversionFactor();
    }

    public static string GetUnitName(this LengthUnit unit)
    {
        return unit.ToString();
    }
}