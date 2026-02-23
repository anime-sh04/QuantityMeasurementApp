namespace QuantityMeasurementApp;

// UC1: Model class representing Feet measurement
public class Feet
{
    private readonly double value;
    public Feet(double value)
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
        Feet other = (Feet)obj;

        return this.value.CompareTo(other.value)==0;
    }
}