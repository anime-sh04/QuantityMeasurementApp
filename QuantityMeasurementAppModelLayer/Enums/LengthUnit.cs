using System;

namespace QuantityMeasurementApp.Enums
{
    public enum LengthUnit
    {
        Feet,
        Inch,
        Yard,
        Centimeter
    }

    public static class LengthUnitExtension
    {
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Feet => 1.0,
                LengthUnit.Inch => 1.0 / 12.0,
                LengthUnit.Yard => 3.0,
                LengthUnit.Centimeter => 1.0 / 30.48,
                _ => throw new ArgumentException("Unsupported Length Unit")
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
        public static void ValidateOperationSupport(this LengthUnit unit, string operation)
        {
            // Length supports all current arithmetic operations.
        }
    }
}