using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Interface;
using Microsoft.Extensions.Logging;

namespace QuantityMeasurementAppRepositoryLayer.Cache
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private readonly List<QuantityMeasurementEntity> _cache = new();
        private int _idCounter = 1;
        private readonly ILogger<QuantityMeasurementCacheRepository> _logger;

        public QuantityMeasurementCacheRepository(ILogger<QuantityMeasurementCacheRepository> logger)
        {
            _logger = logger;
            _logger.LogInformation("CacheRepository initialized (in-memory).");
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            entity.Id = _idCounter++;
            _cache.Add(entity);
            _logger.LogInformation("Saved entity Id={Id} to cache.", entity.Id);
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements() => new(_cache);

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
            => _cache.Where(e => e.OperationType.Equals(operationType, StringComparison.OrdinalIgnoreCase)).ToList();

        public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType)
            => _cache.Where(e => e.MeasurementType.Equals(measurementType, StringComparison.OrdinalIgnoreCase)).ToList();

        public int GetTotalCount() => _cache.Count;

        public void DeleteAll()
        {
            _cache.Clear();
            _idCounter = 1;
            _logger.LogInformation("All cache entries deleted.");
        }

        public string GetPoolStatistics() => $"Cache size: {_cache.Count}";
    }
}
