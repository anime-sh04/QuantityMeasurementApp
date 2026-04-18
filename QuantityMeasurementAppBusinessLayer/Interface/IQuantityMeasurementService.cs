using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppModelLayer.Models;

namespace QuantityMeasurementAppBusinessLayer.Interface
{
    public interface IQuantityMeasurementService
    {
        // Works for all types including Temperature
        bool Compare(QuantityDTO first, QuantityDTO second, int? userId = null);
        QuantityModel Convert(QuantityDTO source, string targetUnit, int? userId = null);

        QuantityModel Add(QuantityDTO first, QuantityDTO second, string targetUnit, int? userId = null);
        QuantityModel Subtract(QuantityDTO first, QuantityDTO second, string targetUnit, int? userId = null);
        QuantityModel Divide(QuantityDTO first, QuantityDTO second, string targetUnit, int? userId = null);

        // History / DB  (global — admin use)
        List<QuantityMeasurementEntity> GetHistory();
        List<QuantityMeasurementEntity> GetHistoryByOperation(string operationType);
        List<QuantityMeasurementEntity> GetHistoryByType(string measurementType);
        int    GetTotalCount();
        void   DeleteAllHistory();
        string GetPoolStatistics();

        // History / DB  (per-user)
        List<QuantityMeasurementEntity> GetHistory(int userId);
        List<QuantityMeasurementEntity> GetHistoryByOperation(string operationType, int userId);
        List<QuantityMeasurementEntity> GetHistoryByType(string measurementType, int userId);
        int  GetTotalCount(int userId);
        void DeleteAllHistory(int userId);
    }
}