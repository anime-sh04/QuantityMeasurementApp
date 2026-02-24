
namespace QuantityMeasurementApp;
// UC2: Model class representing Inches measurement
public class Inches
{
    private readonly double value;
    public Inches(double value)
    {
        this.value = value;
    }
    
    // Override Equals() for value-based comparison
    public override bool Equals(object? obj)
    {
        if(this == obj)
        {
            return true;
        }
        if(obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }
        Inches other = (Inches)obj;

        return this.value.CompareTo(other.value)==0;
    }
}