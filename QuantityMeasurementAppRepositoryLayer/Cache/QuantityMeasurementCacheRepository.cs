using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementAppRepositoryLayer.Cache
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private readonly List<QuantityMeasurementEntity> _storage = new();
        private int _idCounter = 1;

        public void Save(QuantityMeasurementEntity entity)
        {
            entity.Id = _idCounter++;
            _storage.Add(entity);
        }

        public List<QuantityMeasurementEntity> GetAll()
        {
            return _storage;
        }
    }
}