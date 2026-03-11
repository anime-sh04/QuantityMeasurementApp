namespace QuantityMeasurementApp.ModelLayer;

public enum WeightUnit
{
    Kilogram,
    Gram,
    Pound
}

public static class WeightUnitExtensions
{
    public static double GetConversionFactor(this WeightUnit unit)
    {
        return unit switch
        {
            WeightUnit.Kilogram => 1.0,
            WeightUnit.Gram => 0.001,
            WeightUnit.Pound => 0.453592,
            _ => throw new ArgumentException("Unsupported unit")
        };
    }

    public static double ConvertToBaseUnit(this WeightUnit unit, double value)
    {
        return value * unit.GetConversionFactor();
    }

    public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
    {
        return baseValue / unit.GetConversionFactor();
    }

    public static string GetUnitName(this WeightUnit unit)
    {
        return unit.ToString();
    }
}