using ConversionService.Exceptions;
using ConversionService.Repository;
using ConversionService.Util;
using Shared.Models;

namespace ConversionService.Services
{
    public record QuantityInput(double Value, string Unit, string MeasurementType);
    public record QuantityResult(double Value, string Unit);

    public interface IConversionService
    {
        bool          Compare(QuantityInput first, QuantityInput second, int? userId = null);
        QuantityResult Add(QuantityInput first, QuantityInput second, string targetUnit, int? userId = null);
        QuantityResult Subtract(QuantityInput first, QuantityInput second, string targetUnit, int? userId = null);
        QuantityResult Divide(QuantityInput first, QuantityInput second, int? userId = null);
        QuantityResult Convert(QuantityInput source, string targetUnit, int? userId = null);
    }

    public class ConversionServiceImpl : IConversionService
    {
        private readonly IConversionRepository _repository;
        private readonly ILogger<ConversionServiceImpl> _logger;

        public ConversionServiceImpl(IConversionRepository repository, ILogger<ConversionServiceImpl> logger)
        {
            _repository = repository;
            _logger     = logger;
        }

        public bool Compare(QuantityInput first, QuantityInput second, int? userId = null)
        {
            ValidateSameType(first, second, "Compare");
            double v1    = UnitConverter.ToBaseUnit(first.Value, first.Unit, first.MeasurementType);
            double v2    = UnitConverter.ToBaseUnit(second.Value, second.Unit, second.MeasurementType);
            bool   equal = Math.Abs(v1 - v2) < 1e-9;
            string result = equal ? "Equal" : (v1 > v2 ? "First is greater" : "Second is greater");

            _logger.LogInformation("Compare {v1}{u1} vs {v2}{u2} => {r}",
                first.Value, first.Unit, second.Value, second.Unit, result);

            Persist("Compare", first, second, result, userId);
            return equal;
        }

        public QuantityResult Add(QuantityInput first, QuantityInput second, string targetUnit, int? userId = null)
        {
            ValidateSameType(first, second, "Add");
            GuardTemperature(first.MeasurementType, "Add");

            double sum      = UnitConverter.ToBaseUnit(first.Value, first.Unit, first.MeasurementType)
                            + UnitConverter.ToBaseUnit(second.Value, second.Unit, second.MeasurementType);
            double inTarget = UnitConverter.FromBaseUnit(sum, targetUnit, first.MeasurementType);

            _logger.LogInformation("Add {v1}{u1} + {v2}{u2} => {r} {t}",
                first.Value, first.Unit, second.Value, second.Unit, inTarget, targetUnit);

            Persist("Add", first, second, $"{inTarget:G} {targetUnit}", userId);
            return new QuantityResult(inTarget, targetUnit);
        }

        public QuantityResult Subtract(QuantityInput first, QuantityInput second, string targetUnit, int? userId = null)
        {
            ValidateSameType(first, second, "Subtract");
            GuardTemperature(first.MeasurementType, "Subtract");

            double diff     = UnitConverter.ToBaseUnit(first.Value, first.Unit, first.MeasurementType)
                            - UnitConverter.ToBaseUnit(second.Value, second.Unit, second.MeasurementType);
            double inTarget = UnitConverter.FromBaseUnit(diff, targetUnit, first.MeasurementType);

            _logger.LogInformation("Subtract {v1}{u1} - {v2}{u2} => {r} {t}",
                first.Value, first.Unit, second.Value, second.Unit, inTarget, targetUnit);

            Persist("Subtract", first, second, $"{inTarget:G} {targetUnit}", userId);
            return new QuantityResult(inTarget, targetUnit);
        }

        public QuantityResult Divide(QuantityInput first, QuantityInput second, int? userId = null)
        {
            ValidateSameType(first, second, "Divide");
            GuardTemperature(first.MeasurementType, "Divide");

            double v2 = UnitConverter.ToBaseUnit(second.Value, second.Unit, second.MeasurementType);
            if (Math.Abs(v2) < 1e-15)
                throw new ConversionException("Division by zero.");

            double v1       = UnitConverter.ToBaseUnit(first.Value, first.Unit, first.MeasurementType);
            double quotient = v1 / v2;

            _logger.LogInformation("Divide {v1}{u1} / {v2}{u2} => {q}",
                first.Value, first.Unit, second.Value, second.Unit, quotient);

            Persist("Divide", first, second, $"{quotient:G} (ratio)", userId);
            return new QuantityResult(quotient, "ratio");
        }

        public QuantityResult Convert(QuantityInput source, string targetUnit, int? userId = null)
        {
            if (string.IsNullOrWhiteSpace(targetUnit))
                throw new ConversionException("Target unit cannot be empty.");

            double baseVal   = UnitConverter.ToBaseUnit(source.Value, source.Unit, source.MeasurementType);
            double converted = UnitConverter.FromBaseUnit(baseVal, targetUnit, source.MeasurementType);

            _logger.LogInformation("Convert {v}{u} => {c} {t}",
                source.Value, source.Unit, converted, targetUnit);

            Persist("Convert",
                source,
                new QuantityInput(converted, targetUnit, source.MeasurementType),
                $"{converted:G} {targetUnit}",
                userId);

            return new QuantityResult(converted, targetUnit);
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static void ValidateSameType(QuantityInput a, QuantityInput b, string op)
        {
            if (!a.MeasurementType.Equals(b.MeasurementType, StringComparison.OrdinalIgnoreCase))
                throw new ConversionException(
                    $"{op}: Cannot mix '{a.MeasurementType}' and '{b.MeasurementType}'.");
        }

        private static void GuardTemperature(string type, string op)
        {
            if (type.Equals("Temperature", StringComparison.OrdinalIgnoreCase))
                throw new ConversionException(
                    $"'{op}' is not supported for Temperature. Use Compare or Convert instead.");
        }

        private void Persist(string op, QuantityInput a, QuantityInput b, string result, int? userId)
        {
            try
            {
                _repository.Save(new QuantityMeasurementEntity
                {
                    OperationType   = op,
                    MeasurementType = a.MeasurementType,
                    FirstValue      = a.Value,
                    FirstUnit       = a.Unit,
                    SecondValue     = b.Value,
                    SecondUnit      = b.Unit,
                    Result          = result,
                    CreatedAt       = DateTime.UtcNow,
                    UserId          = userId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist entity for {op}.", op);
            }
        }
    }
}
