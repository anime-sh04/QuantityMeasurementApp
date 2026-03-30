using QuantityMeasurementAppModelLayer.Models;

namespace QuantityMeasurementAppRepositoryLayer.Interface
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAllMeasurements();
        List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType);
        List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType);
        int GetTotalCount();
        void DeleteAll();

        string GetPoolStatistics() => "N/A - No connection pool";
        void ReleaseResources() { /* default no-op */ }
    }
}
