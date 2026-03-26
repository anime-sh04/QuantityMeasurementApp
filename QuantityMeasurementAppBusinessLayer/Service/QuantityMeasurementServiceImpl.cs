using QuantityMeasurementAppBusinessLayer.Exception;
using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppBusinessLayer.Util;
using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Interface;
using Microsoft.Extensions.Logging;

namespace QuantityMeasurementAppBusinessLayer.Service
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;
        private readonly ILogger<QuantityMeasurementServiceImpl> _logger;

        public QuantityMeasurementServiceImpl(
            IQuantityMeasurementRepository repository,
            ILogger<QuantityMeasurementServiceImpl> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
            _logger.LogInformation("QuantityMeasurementService initialized.");
        }

        // ── Compare (all types) ───────────────────────────────────────────────────
        public bool Compare(QuantityDTO first, QuantityDTO second)
        {
            ValidateSameType(first, second, "Compare");
            double v1 = UnitConverter.ToBaseUnit(first.Value,  first.Unit,  first.MeasurementType);
            double v2 = UnitConverter.ToBaseUnit(second.Value, second.Unit, second.MeasurementType);
            bool equal = Math.Abs(v1 - v2) < 1e-9;
            string result = equal ? "Equal" : (v1 > v2 ? "First is greater" : "Second is greater");
            _logger.LogInformation("Compare {v1}{u1} vs {v2}{u2} ({t}) => {r}",
                first.Value, first.Unit, second.Value, second.Unit, first.MeasurementType, result);
            PersistEntity("Compare", first, second, result);
            return equal;
        }

        // ── Add (non-temperature) ─────────────────────────────────────────────────
        public QuantityModel Add(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            ValidateSameType(first, second, "Add");
            GuardTemperature(first.MeasurementType, "Add");
            double sum = UnitConverter.ToBaseUnit(first.Value, first.Unit, first.MeasurementType)
                       + UnitConverter.ToBaseUnit(second.Value, second.Unit, second.MeasurementType);
            double inTarget = UnitConverter.FromBaseUnit(sum, targetUnit, first.MeasurementType);
            string result = $"{inTarget:G} {targetUnit}";
            _logger.LogInformation("Add {v1}{u1} + {v2}{u2} => {r}", first.Value, first.Unit, second.Value, second.Unit, result);
            PersistEntity("Add", first, second, result);
            return new QuantityModel(inTarget, targetUnit);
        }

        // ── Subtract (non-temperature) ────────────────────────────────────────────
        public QuantityModel Subtract(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            ValidateSameType(first, second, "Subtract");
            GuardTemperature(first.MeasurementType, "Subtract");
            double diff = UnitConverter.ToBaseUnit(first.Value, first.Unit, first.MeasurementType)
                        - UnitConverter.ToBaseUnit(second.Value, second.Unit, second.MeasurementType);
            double inTarget = UnitConverter.FromBaseUnit(diff, targetUnit, first.MeasurementType);
            string result = $"{inTarget:G} {targetUnit}";
            _logger.LogInformation("Subtract {v1}{u1} - {v2}{u2} => {r}", first.Value, first.Unit, second.Value, second.Unit, result);
            PersistEntity("Subtract", first, second, result);
            return new QuantityModel(inTarget, targetUnit);
        }

        // ── Divide (non-temperature) ──────────────────────────────────────────────
        public QuantityModel Divide(QuantityDTO first, QuantityDTO second, string targetUnit)
        {
            ValidateSameType(first, second, "Divide");
            GuardTemperature(first.MeasurementType, "Divide");
            double v2 = UnitConverter.ToBaseUnit(second.Value, second.Unit, second.MeasurementType);
            if (Math.Abs(v2) < 1e-15)
                throw new QuantityMeasurementException("Division by zero.");
            double v1 = UnitConverter.ToBaseUnit(first.Value, first.Unit, first.MeasurementType);
            double quotient = v1 / v2;
            // Divide is dimensionless, targetUnit used only for display label
            string result = $"{quotient:G} (ratio)";
            _logger.LogInformation("Divide {v1}{u1} / {v2}{u2} => {r}", first.Value, first.Unit, second.Value, second.Unit, result);
            PersistEntity("Divide", first, second, result);
            return new QuantityModel(quotient, "ratio");
        }

        // ── Convert (all types) ───────────────────────────────────────────────────
        public QuantityModel Convert(QuantityDTO source, string targetUnit)
        {
            if (string.IsNullOrWhiteSpace(targetUnit))
                throw new QuantityMeasurementException("Target unit cannot be empty.");
            double baseVal   = UnitConverter.ToBaseUnit(source.Value, source.Unit, source.MeasurementType);
            double converted = UnitConverter.FromBaseUnit(baseVal, targetUnit, source.MeasurementType);
            string result    = $"{converted:G} {targetUnit}";
            _logger.LogInformation("Convert {v}{u} => {r}", source.Value, source.Unit, result);
            PersistEntity("Convert", source, new QuantityDTO(converted, targetUnit, source.MeasurementType), result);
            return new QuantityModel(converted, targetUnit);
        }

        // ── History ───────────────────────────────────────────────────────────────
        public List<QuantityMeasurementEntity> GetHistory()               => _repository.GetAllMeasurements();
        public List<QuantityMeasurementEntity> GetHistoryByOperation(string op)   => _repository.GetMeasurementsByOperation(op);
        public List<QuantityMeasurementEntity> GetHistoryByType(string type)      => _repository.GetMeasurementsByType(type);
        public int    GetTotalCount()    => _repository.GetTotalCount();
        public void   DeleteAllHistory() => _repository.DeleteAll();
        public string GetPoolStatistics() => _repository.GetPoolStatistics();

        // ── Helpers ───────────────────────────────────────────────────────────────
        private static void ValidateSameType(QuantityDTO a, QuantityDTO b, string op)
        {
            if (!a.MeasurementType.Equals(b.MeasurementType, StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException(
                    $"{op}: Cannot mix '{a.MeasurementType}' and '{b.MeasurementType}'.");
        }

        private static void GuardTemperature(string type, string op)
        {
            if (type.Equals("Temperature", StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException(
                    $"'{op}' is not supported for Temperature. Use Compare or Convert instead.");
        }

        private void PersistEntity(string op, QuantityDTO a, QuantityDTO b, string result)
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
                    CreatedAt       = DateTime.UtcNow
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to persist entity for {op}.", op);
            }
        }
    }
}
