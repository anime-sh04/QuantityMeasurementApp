namespace QuantityMeasurementApp.ModelLayer;

public enum WeightUnit
{
    Kilogram,
    Gram,
    Pound
}

public static class WeightUnitExtension
{
    // Convert weight → base unit (Kilogram)
    public static double ConvertToBaseUnit(this WeightUnit unit, double value)
    {
        return unit switch
        {
            WeightUnit.Kilogram => value,
            WeightUnit.Gram => value * 0.001,
            WeightUnit.Pound => value * 0.453592,
            _ => throw new ArgumentException("Unsupported weight unit")
        };
    }

    // Convert base unit (Kilogram) → target unit
    public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
    {
        return unit switch
        {
            WeightUnit.Kilogram => baseValue,
            WeightUnit.Gram => baseValue / 0.001,
            WeightUnit.Pound => baseValue / 0.453592,
            _ => throw new ArgumentException("Unsupported weight unit")
        };
    }
}