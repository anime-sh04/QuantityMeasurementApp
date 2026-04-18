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

        public List<QuantityMeasurementEntity> GetAllMeasurements()
            => new(_cache.OrderByDescending(e => e.CreatedAt).ToList());

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
            => _cache.Where(e => e.OperationType.Equals(operationType, StringComparison.OrdinalIgnoreCase))
                     .OrderByDescending(e => e.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType)
            => _cache.Where(e => e.MeasurementType.Equals(measurementType, StringComparison.OrdinalIgnoreCase))
                     .OrderByDescending(e => e.CreatedAt).ToList();

        public int GetTotalCount() => _cache.Count;

        public void DeleteAll()
        {
            _cache.Clear();
            _idCounter = 1;
            _logger.LogInformation("All cache entries deleted.");
        }

        public string GetPoolStatistics() => $"Cache size: {_cache.Count}";

        // ── Per-user overloads ────────────────────────────────────────────────
        public List<QuantityMeasurementEntity> GetAllMeasurements(int userId)
            => _cache.Where(e => e.UserId == userId)
                     .OrderByDescending(e => e.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType, int userId)
            => _cache.Where(e => e.UserId == userId &&
                                 e.OperationType.Equals(operationType, StringComparison.OrdinalIgnoreCase))
                     .OrderByDescending(e => e.CreatedAt).ToList();

        public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType, int userId)
            => _cache.Where(e => e.UserId == userId &&
                                 e.MeasurementType.Equals(measurementType, StringComparison.OrdinalIgnoreCase))
                     .OrderByDescending(e => e.CreatedAt).ToList();

        public int GetTotalCount(int userId)
            => _cache.Count(e => e.UserId == userId);

        public void DeleteAll(int userId)
        {
            _cache.RemoveAll(e => e.UserId == userId);
            _logger.LogInformation("Cache entries for userId={UserId} deleted.", userId);
        }
    }
}