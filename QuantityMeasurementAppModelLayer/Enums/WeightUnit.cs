using System;

namespace QuantityMeasurementApp.Enums
{
    public enum WeightUnit
    {
        Kilogram,
        Gram,
        Pound
    }
    public static class WeightUnitExtension
    {
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.Kilogram => 1.0,
                WeightUnit.Gram => 0.001,
                WeightUnit.Pound => 0.453592,
                _ => throw new ArgumentException("Unsupported Weight Unit")
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
        public static void ValidateOperationSupport(this WeightUnit unit, string operation)
        {
            // Weight supports all current arithmetic operations.
        }
    }
}