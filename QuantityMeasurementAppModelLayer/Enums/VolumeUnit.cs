using System;

namespace QuantityMeasurementApp.Enums
{
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
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Unsupported volume unit.")
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

        public static string GetUnitName(this VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.Litre => "Litre",
                VolumeUnit.Millilitre => "Millilitre",
                VolumeUnit.Gallon => "Gallon",
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Unsupported volume unit.")
            };
        }


        public static void ValidateOperationSupport(this VolumeUnit unit, string operation)
        {
            // Volume supports all current arithmetic operations.
        }
    }
}