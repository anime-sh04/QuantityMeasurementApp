namespace QuantityMeasurementApp.ModelLayer;

public enum TemperatureUnit
{
    Celsius,
    Fahrenheit,
    Kelvin
}

public static class TemperatureUnitExtensions
{
    public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
    {
        switch (unit)
        {
            case TemperatureUnit.Celsius:
                return value;

            case TemperatureUnit.Fahrenheit:
                return (value - 32) * 5 / 9;

            case TemperatureUnit.Kelvin:
                return value - 273.15;

            default:
                throw new ArgumentException("Unsupported temperature unit");
        }
    }

    public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
    {
        switch (unit)
        {
            case TemperatureUnit.Celsius:
                return baseValue;

            case TemperatureUnit.Fahrenheit:
                return baseValue * 9 / 5 + 32;

            case TemperatureUnit.Kelvin:
                return baseValue + 273.15;

            default:
                throw new ArgumentException("Unsupported temperature unit");
        }
    }

    public static string GetUnitName(this TemperatureUnit unit)
    {
        return unit.ToString();
    }

    public static bool SupportsArithmetic(this TemperatureUnit unit)
    {
        return false;
    }

    public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
    {
        throw new NotSupportedException(
            $"Temperature does not support '{operation}' operation.");
    }
}