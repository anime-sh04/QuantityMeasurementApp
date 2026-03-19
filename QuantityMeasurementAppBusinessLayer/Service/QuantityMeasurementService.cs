using QuantityMeasurementAppBusinessLayer.Exception;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementServices.Services
{
    /// UC1–UC2:   Feet and Inch equality.
    /// UC3:       Feet - Inch conversion equality (1 ft = 12 in).
    /// UC4:       Yard - Feet conversion equality (1 yd = 3 ft).
    /// UC5:       Centimeter - Feet conversion equality.
    /// UC6:       Kilogram - Gram conversion equality.
    /// UC7:       Length addition — result in base unit.
    /// UC8:       Weight addition — kg + g result in grams.
    /// UC9–UC10:  Generic multi-category operations via string-based dispatch.
    /// UC11:      Volume units — litre, millilitre, gallon.
    /// UC12:      Subtract and Divide operations.
    /// UC13:      All arithmetic routed through a single ApplyArithmetic helper (DRY).
    /// UC14:      Temperature equality/conversion only; arithmetic throws.
    
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementService(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
        }

        
        // UC1–UC6, UC9–UC11, UC14    Compare

        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            Validate(first);
            Validate(second);
            EnsureSameCategory(first, second, "compare");

            double a = ConvertToBase(first);
            double b = ConvertToBase(second);

            PersistBoth(first, second);

            return Math.Abs(a - b) < 1e-6;
        }

        public QuantityModel Convert(QuantityDTO source, string targetUnit)
        {
            Validate(source);
        
            double baseValue = ConvertToBase(source);
            double convertedValue = ConvertFromBase(baseValue, source.Category, targetUnit);
        
            Persist(source);
        
            return new QuantityModel
            {
                BaseValue = convertedValue,
                Unit = targetUnit,
                Category = source.Category
            };
        }

        // UC7–UC8, UC13  →  Add  
        public QuantityModel Add(QuantityDTO first, QuantityDTO second, string targetUnit = null)
        {
            return ApplyArithmetic(first, second, "addition", (a, b) => a + b, targetUnit);
        }

        // UC12  →  Subtract / Divide  
        public QuantityModel Subtract(QuantityDTO first, QuantityDTO second, string targetUnit = null)
        {
            return ApplyArithmetic(first, second, "subtraction", (a, b) => a - b, targetUnit);
        }

        public double Divide(QuantityDTO first, QuantityDTO second)
        {
            Validate(first);
            Validate(second);
            EnsureSameCategory(first, second, "division");
            EnsureArithmeticAllowed(first, "division");

            double a = ConvertToBase(first);
            double b = ConvertToBase(second);

            if (Math.Abs(b) < 1e-12)
                throw new QuantityMeasurementException("Division by zero is not allowed.");

            PersistBoth(first, second);
            return a / b;
        }

        // History
        public List<QuantityMeasurementEntity> GetHistory()
        {
            return _repository.GetAll();
        }

        // UC13 —  arithmetic helper


        /// <summary>
        /// Central arithmetic dispatcher used by Add and Subtract.
        /// All validation, category checks, temperature guard, conversion,
        /// persistence, and result packaging happen exactly once here.
        /// </summary>
        private QuantityModel ApplyArithmetic(QuantityDTO first,QuantityDTO second,string operationName,Func<double, double, double> operation,string targetUnit = null)
        {
            Validate(first);
            Validate(second);
            EnsureSameCategory(first, second, operationName);
            EnsureArithmeticAllowed(first, operationName);

            PersistBoth(first, second);

            //  Target unit provided
            if (!string.IsNullOrWhiteSpace(targetUnit))
            {
                double a = ConvertToBase(first);
                double b = ConvertToBase(second);

                double resultBase = operation(a, b);

                double converted = ConvertFromBase(resultBase, first.Category, targetUnit);

                return new QuantityModel
                {
                    BaseValue = converted,
                    Unit = targetUnit,
                    Category = first.Category
                };
            }

            //  No target unit and units must match
            if (!first.Unit.Equals(second.Unit, StringComparison.OrdinalIgnoreCase))
            {
                throw new QuantityMeasurementException(
                    $"Units must be the same when no target unit is provided. " +
                    $"Got '{first.Unit}' and '{second.Unit}'.");
            }

            // Direct arithmetic (NO conversion)
            double result = operation(first.Value, second.Value);

            return new QuantityModel
            {
                BaseValue = result,
                Unit = first.Unit,
                Category = first.Category
            };
        }

        // Conversion — UC3–UC8, UC11, UC14

        /// <summary>
        /// Converts any supported quantity to its category base unit.
        ///
        /// Length base  : Inch (UC3–UC5, UC7)
        /// Weight base  : Gram (UC6, UC8)
        /// Volume base  : Milliliter (UC11)
        /// Temperature  : Celsius (UC14; arithmetic blocked separately)
        /// </summary>
        private double ConvertToBase(QuantityDTO dto)
        {
            string category = dto.Category.Trim().ToLowerInvariant();
            string unit     = dto.Unit.Trim().ToLowerInvariant();

            return category switch
            {
                // UC1–UC5, UC7
                "length" => unit switch
                {
                    "inch"        => dto.Value,
                    "feet"        => dto.Value * 12.0,
                    "yard"        => dto.Value * 36.0,
                    "centimeter"  => dto.Value / 2.54,
                    _ => throw new QuantityMeasurementException($"Unsupported length unit: '{dto.Unit}'.")
                },

                // UC6, UC8
                "weight" => unit switch
                {
                    "gram"      => dto.Value,
                    "kilogram"  => dto.Value * 1000.0,
                    "pound"     => dto.Value * 453.592,
                    _ => throw new QuantityMeasurementException($"Unsupported weight unit: '{dto.Unit}'.")
                },

                // UC11
                "volume" => unit switch
                {
                    "milliliter" or "millilitre" => dto.Value,
                    "liter"      or "litre"      => dto.Value * 1000.0,
                    "gallon"                     => dto.Value * 3785.41,
                    _ => throw new QuantityMeasurementException($"Unsupported volume unit: '{dto.Unit}'.")
                },

                // UC14 — conversion only; arithmetic is blocked before this point
                "temperature" => unit switch
                {
                    "celsius"    => dto.Value,
                    "fahrenheit" => (dto.Value - 32.0) * 5.0 / 9.0,
                    "kelvin"     => dto.Value - 273.15,
                    _ => throw new QuantityMeasurementException($"Unsupported temperature unit: '{dto.Unit}'.")
                },

                _ => throw new QuantityMeasurementException($"Unsupported category: '{dto.Category}'.")
            };
        }
        private double ConvertFromBase(double baseValue, string category, string targetUnit)
        {
            category = category.Trim().ToLowerInvariant();
            targetUnit = targetUnit.Trim().ToLowerInvariant();

            return category switch
            {
                "length" => targetUnit switch
                {
                    "inch"        => baseValue,
                    "feet"        => baseValue / 12.0,
                    "yard"        => baseValue / 36.0,
                    "centimeter"  => baseValue * 2.54,
                    _ => throw new QuantityMeasurementException($"Unsupported length unit: '{targetUnit}'.")
                },

                "weight" => targetUnit switch
                {
                    "gram"      => baseValue,
                    "kilogram"  => baseValue / 1000.0,
                    "pound"     => baseValue / 453.592,
                    _ => throw new QuantityMeasurementException($"Unsupported weight unit: '{targetUnit}'.")
                },

                "volume" => targetUnit switch
                {
                    "milliliter" or "millilitre" => baseValue,
                    "liter"      or "litre"      => baseValue / 1000.0,
                    "gallon"                     => baseValue / 3785.41,
                    _ => throw new QuantityMeasurementException($"Unsupported volume unit: '{targetUnit}'.")
                },

                "temperature" => targetUnit switch
                {
                    "celsius"    => baseValue,
                    "fahrenheit" => (baseValue * 9.0 / 5.0) + 32.0,
                    "kelvin"     => baseValue + 273.15,
                    _ => throw new QuantityMeasurementException($"Unsupported temperature unit: '{targetUnit}'.")
                },

                _ => throw new QuantityMeasurementException($"Unsupported category: '{category}'.")
            };
        }



        // Guards & helpers

        private void Validate(QuantityDTO dto)
        {
            if (dto == null)
                throw new QuantityMeasurementException("Quantity cannot be null.");
            if (string.IsNullOrWhiteSpace(dto.Category))
                throw new QuantityMeasurementException("Category is required.");
            if (string.IsNullOrWhiteSpace(dto.Unit))
                throw new QuantityMeasurementException("Unit is required.");
        }

        private void EnsureSameCategory(QuantityDTO first, QuantityDTO second, string operation)
        {
            if (!first.Category.Equals(second.Category, StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException(
                    $"Cannot {operation} quantities of different categories " +
                    $"('{first.Category}' vs '{second.Category}').");
        }


        // UC14 — Blocks arithmetic for temperature; leaves all others unchanged.
        private void EnsureArithmeticAllowed(QuantityDTO dto, string operation)
        {
            if (dto.Category.Trim().Equals("Temperature", StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException(
                    $"Temperature does not support {operation}. " +
                    "Only comparison and conversion are supported for temperature measurements.");
        }

        private void PersistBoth(QuantityDTO first, QuantityDTO second)
        {
            Persist(first);
            Persist(second);
        }

        private void Persist(QuantityDTO dto)
        {
            _repository.Save(new QuantityMeasurementEntity
            {
                Value    = dto.Value,
                Unit     = dto.Unit,
                Category = dto.Category
            });
        }

        private static string GetBaseUnit(string category) =>
            category.Trim().ToLowerInvariant() switch
            {
                "length"      => "Inch",
                "weight"      => "Gram",
                "volume"      => "Milliliter",
                "temperature" => "Celsius",
                _             => "Unknown"
            };
    }
}