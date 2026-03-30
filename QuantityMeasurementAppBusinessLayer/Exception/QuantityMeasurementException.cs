using System;

namespace QuantityMeasurementAppBusinessLayer.Exception
{
    public class QuantityMeasurementException : ApplicationException
    {
        public QuantityMeasurementException(string message) : base(message)
        {
        }
    }
}