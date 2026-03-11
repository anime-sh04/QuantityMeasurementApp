namespace QuantityMeasurementApp.ModelLayer;

public enum VolumeUnit
{
    Litre,
    Millilitre,
    Gallon
}

public static class VolumeUnitExtensions
{
    public static double GetConversionFactor(this VolumeUnit unit)
    {
        return unit switch
        {
            VolumeUnit.Litre => 1.0,
            VolumeUnit.Millilitre => 0.001,
            VolumeUnit.Gallon => 3.78541,
            _ => throw new ArgumentException("Unsupported volume unit")
        };
    }

    public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
    {
        return value * unit.GetConversionFactor();
    }

    public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
    {
        return baseValue / unit.GetConversionFactor();
    }
}