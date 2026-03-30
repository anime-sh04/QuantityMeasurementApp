using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppModelLayer.Models;

namespace QuantityMeasurementAppBusinessLayer.Interface
{
    public interface IQuantityMeasurementService
    {
        // Works for all types including Temperature
        bool Compare(QuantityDTO first, QuantityDTO second);
        QuantityModel Convert(QuantityDTO source, string targetUnit);

        QuantityModel Add(QuantityDTO first, QuantityDTO second, string targetUnit);
        QuantityModel Subtract(QuantityDTO first, QuantityDTO second, string targetUnit);
        QuantityModel Divide(QuantityDTO first, QuantityDTO second, string targetUnit);

        // History / DB
        List<QuantityMeasurementEntity> GetHistory();
        List<QuantityMeasurementEntity> GetHistoryByOperation(string operationType);
        List<QuantityMeasurementEntity> GetHistoryByType(string measurementType);
        int    GetTotalCount();
        void   DeleteAllHistory();
        string GetPoolStatistics();
    }
}
