using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Api.DTOs
{
    
    
    public class QuantityRequest
    {
        
        [Required]
        public double Value { get; set; }

        
        [Required]
        [StringLength(50)]
        public string Unit { get; set; } = string.Empty;

        
        [Required]
        [StringLength(50)]
        public string MeasurementType { get; set; } = string.Empty;
    }

    public class CompareRequestDTO
    {
        
        [Required]
        public QuantityRequest QuantityOne { get; set; } = new();

        
        [Required]
        public QuantityRequest QuantityTwo { get; set; } = new();
    }
    public class AddRequestDTO
    {
        
        [Required]
        public QuantityRequest QuantityOne { get; set; } = new();

         [Required]
        public QuantityRequest QuantityTwo { get; set; } = new();

        [Required]
        [StringLength(50)]
        public string TargetUnit { get; set; } = string.Empty;
    }

    public class SubtractRequestDTO
    {
        [Required]
        public QuantityRequest QuantityOne { get; set; } = new();

        [Required]
        public QuantityRequest QuantityTwo { get; set; } = new();
        [Required]
        [StringLength(50)]
        public string TargetUnit { get; set; } = string.Empty;
    }
    public class DivideRequestDTO
    {
        [Required]
        public QuantityRequest QuantityOne { get; set; } = new();

        [Required]
        public QuantityRequest QuantityTwo { get; set; } = new();
    }
    public class ConvertRequestDTO
    {
        [Required]
        public QuantityRequest Source { get; set; } = new();
        [Required]
        [StringLength(50)]
        public string TargetUnit { get; set; } = string.Empty;
    }
}
