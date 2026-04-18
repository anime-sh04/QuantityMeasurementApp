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

        // Per-user overloads
        List<QuantityMeasurementEntity> GetAllMeasurements(int userId);
        List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType, int userId);
        List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType, int userId);
        int GetTotalCount(int userId);
        void DeleteAll(int userId);

        string GetPoolStatistics() => "N/A - No connection pool";
        void ReleaseResources() { /* default no-op */ }
    }
}