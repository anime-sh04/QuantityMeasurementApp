namespace QuantityMeasurementAppBusinessLayer.Util
{
    public static class UnitConverter
    {
        
        public static readonly Dictionary<string, double> LengthToCm = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Feet",        30.48  },
            { "Inches",       2.54  },
            { "Yards",       91.44  },
            { "Centimeters",  1.0   }
        };

        
        public static readonly Dictionary<string, double> WeightToKg = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Kilogram", 1.0       },
            { "Gram",     0.001     },
            { "Pound",    0.453592  }
        };

        
        public static readonly Dictionary<string, double> VolumeToLitre = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Litre",      1.0      },
            { "Millilitre", 0.001    },
            { "Gallon",     3.78541  }
        };

        
        public static string[] UnitsFor(string measurementType) =>
            measurementType.ToLower() switch
            {
                "length"      => ["Feet", "Inches", "Yards", "Centimeters"],
                "weight"      => ["Kilogram", "Gram", "Pound"],
                "volume"      => ["Litre", "Millilitre", "Gallon"],
                "temperature" => ["Celsius", "Fahrenheit", "Kelvin"],
                _ => []
            };

        
        public static double ToBaseUnit(double value, string unit, string measurementType) =>
            measurementType.ToLower() switch
            {
                "length"      => Factor(value, unit, LengthToCm,    "Length"),
                "weight"      => Factor(value, unit, WeightToKg,    "Weight"),
                "volume"      => Factor(value, unit, VolumeToLitre, "Volume"),
                "temperature" => ToCelsius(value, unit),
                _ => throw new ArgumentException($"Unknown measurement type: {measurementType}")
            };

        
        public static double FromBaseUnit(double baseValue, string targetUnit, string measurementType) =>
            measurementType.ToLower() switch
            {
                "length"      => baseValue / LengthToCm[targetUnit],
                "weight"      => baseValue / WeightToKg[targetUnit],
                "volume"      => baseValue / VolumeToLitre[targetUnit],
                "temperature" => FromCelsius(baseValue, targetUnit),
                _ => throw new ArgumentException($"Unknown measurement type: {measurementType}")
            };

        public static string BaseUnitName(string measurementType) =>
            measurementType.ToLower() switch
            {
                "length"      => "Centimeters",
                "weight"      => "Kilogram",
                "volume"      => "Litre",
                "temperature" => "Celsius",
                _ => "unit"
            };

        private static double Factor(double value, string unit,
            Dictionary<string, double> map, string type)
        {
            if (!map.TryGetValue(unit, out double f))
                throw new ArgumentException($"Unknown {type} unit: '{unit}'");
            return value * f;
        }

        private static double ToCelsius(double v, string unit) =>
            unit.ToLower() switch
            {
                "celsius"    => v,
                "fahrenheit" => (v - 32) * 5.0 / 9.0,
                "kelvin"     => v - 273.15,
                _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
            };

        private static double FromCelsius(double c, string unit) =>
            unit.ToLower() switch
            {
                "celsius"    => c,
                "fahrenheit" => c * 9.0 / 5.0 + 32,
                "kelvin"     => c + 273.15,
                _ => throw new ArgumentException($"Unknown temperature unit: {unit}")
            };
    }
}
