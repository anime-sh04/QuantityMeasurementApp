using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppModelLayer.Models;

namespace QuantityMeasurementAppBusinessLayer.Interface
{
    
    public interface IQuantityMeasurementService
    {
        bool Compare(QuantityDTO first, QuantityDTO second);
        QuantityModel Convert(QuantityDTO source, string targetUnit);
        QuantityModel Add(QuantityDTO first, QuantityDTO second, string targetUnit);
        QuantityModel Subtract(QuantityDTO first, QuantityDTO second, string targetUnit);
        double Divide(QuantityDTO first, QuantityDTO second);
        List<QuantityMeasurementEntity> GetHistory();
    }
}