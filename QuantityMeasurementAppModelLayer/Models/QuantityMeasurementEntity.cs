using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementAppModelLayer.Models
{
    [Table("QuantityMeasurements")]
    public class QuantityMeasurementEntity
    {
        [Key]
        public int Id { get; set; }

        public string OperationType { get; set; } = string.Empty;   // "Compare", "Add", "Subtract", "Divide", "Convert"
        public string MeasurementType { get; set; } = string.Empty; // "Length", "Weight", "Volume", "Temperature"
        public double FirstValue { get; set; }
        public string FirstUnit { get; set; } = string.Empty;
        public double SecondValue { get; set; }
        public string SecondUnit { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {OperationType} | {MeasurementType} | " +
                   $"{FirstValue} {FirstUnit} vs {SecondValue} {SecondUnit} = {Result} | {CreatedAt:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
