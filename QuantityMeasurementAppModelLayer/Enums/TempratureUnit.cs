using System;

namespace QuantityMeasurementApp.Enums
{
    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }


    public static class TemperatureUnitExtensions
    {
        public static double GetConversionFactor(this TemperatureUnit unit)
        {
            return 1.0;
        }

        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => value,
                TemperatureUnit.Fahrenheit => (value - 32.0) * 5.0 / 9.0,
                TemperatureUnit.Kelvin => value - 273.15,
                _ => throw new ArgumentException("Invalid temperature unit.")
            };
        }
        
        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => baseValue,
                TemperatureUnit.Fahrenheit => (baseValue * 9.0 / 5.0) + 32.0,
                TemperatureUnit.Kelvin => baseValue + 273.15,
                _ => throw new ArgumentException("Invalid temperature unit.")
            };
        }

        public static string GetUnitName(this TemperatureUnit unit)
        {
            return unit switch
            {
                TemperatureUnit.Celsius => "Celsius",
                TemperatureUnit.Fahrenheit => "Fahrenheit",
                TemperatureUnit.Kelvin => "Kelvin",
                _ => throw new ArgumentException("Invalid temperature unit.")
            };
        }

        public static bool SupportsArithmetic(this TemperatureUnit unit)
        {
            return false;
        }

        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            throw new UnsupportedOperationException(
                $"Temperature does not support {operation}. " +
                "Only comparison and conversion are supported for temperature measurements.");
        }
    }

    public class UnsupportedOperationException : Exception
    {
        public UnsupportedOperationException(string message) : base(message)
        {
        }
    }
}