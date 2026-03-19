using QuantityMeasurementAppBusinessLayer.Interface;
using QuantityMeasurementAppModelLayer.DTOs;
using QuantityMeasurementAppModelLayer.Models;

namespace QuantityMeasurementApp.Console.Controller
{
    // Thin controller that delegates all calls to the service layer.
    // UC7–UC8: Add, UC12: Subtract / Divide.
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        public bool CompareQuantities(QuantityDTO first, QuantityDTO second)
            => _service.Compare(first, second);
        
        public QuantityModel ConvertQuantity(QuantityDTO source, string targetUnit)
            => _service.Convert(source, targetUnit);
        public QuantityModel AddQuantities(QuantityDTO first, QuantityDTO second, string targetUnit)
            => _service.Add(first, second, targetUnit);

        //UC12 — Subtract
        public QuantityModel SubtractQuantities(QuantityDTO first, QuantityDTO second, string targetUnit)
            => _service.Subtract(first, second, targetUnit);

        //UC12 — Divide; returns dimensionless ratio
        public double DivideQuantities(QuantityDTO first, QuantityDTO second)
            => _service.Divide(first, second);

        public List<QuantityMeasurementEntity> GetAllHistory()
            => _service.GetHistory();
    }
}