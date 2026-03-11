namespace QuantityMeasurementApp.ModelLayer;

public class Quantity<U> where U : Enum
{
    private readonly double value;
    private readonly U unit;

    public Quantity(double value, U unit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid value");

        if (unit == null)
            throw new ArgumentException("Unit cannot be null");

        this.value = value;
        this.unit = unit;
    }

    public double GetValue()
    {
        return value;
    }

    public U GetUnit()
    {
        return unit;
    }

    // --------------------
    // Convert
    // --------------------
    public Quantity<U> ConvertTo(U targetUnit)
    {
        if (targetUnit == null)
            throw new ArgumentException("Target unit cannot be null");

        double baseValue = ConvertToBase(value, unit);
        double converted = ConvertFromBase(baseValue, targetUnit);

        converted = Math.Round(converted, 2);

        return new Quantity<U>(converted, targetUnit);
    }

    // --------------------
    // Add
    // --------------------
    public Quantity<U> Add(Quantity<U> other)
    {
        return Add(other, this.unit);
    }

    public Quantity<U> Add(Quantity<U> other, U targetUnit)
    {
        if (other == null)
            throw new ArgumentException("Quantity cannot be null");

        if (targetUnit == null)
            throw new ArgumentException("Target unit cannot be null");

        double v1 = ConvertToBase(this.value, this.unit);
        double v2 = ConvertToBase(other.value, other.unit);

        double sum = v1 + v2;

        double result = ConvertFromBase(sum, targetUnit);

        result = Math.Round(result, 2);

        return new Quantity<U>(result, targetUnit);
    }

    // --------------------
    // Subtract (UC12)
    // --------------------
    public Quantity<U> Subtract(Quantity<U> other)
    {
        return Subtract(other, this.unit);
    }

    public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
    {
        if (other == null)
            throw new ArgumentException("Quantity cannot be null");

        if (targetUnit == null)
            throw new ArgumentException("Target unit cannot be null");

        double v1 = ConvertToBase(this.value, this.unit);
        double v2 = ConvertToBase(other.value, other.unit);

        double diff = v1 - v2;

        double result = ConvertFromBase(diff, targetUnit);

        result = Math.Round(result, 2);

        return new Quantity<U>(result, targetUnit);
    }

    // --------------------
    // Divide (UC12)
    // --------------------
    public double Divide(Quantity<U> other)
    {
        if (other == null)
            throw new ArgumentException("Quantity cannot be null");

        double v1 = ConvertToBase(this.value, this.unit);
        double v2 = ConvertToBase(other.value, other.unit);

        if (v2 == 0)
            throw new ArithmeticException("Division by zero");

        return v1 / v2;
    }

    // --------------------
    // Equality
    // --------------------
    public override bool Equals(object obj)
    {
        if (this == obj)
            return true;

        if (obj == null || obj.GetType() != typeof(Quantity<U>))
            return false;

        Quantity<U> other = (Quantity<U>)obj;

        double v1 = ConvertToBase(this.value, this.unit);
        double v2 = ConvertToBase(other.value, other.unit);

        const double EPSILON = 1e-6;

        return Math.Abs(v1 - v2) < EPSILON;
    }

    public override int GetHashCode()
    {
        return ConvertToBase(value, unit).GetHashCode();
    }

    public override string ToString()
    {
        return $"Quantity({value}, {unit})";
    }

    // --------------------
    // Helpers
    // --------------------
    private double ConvertToBase(double value, U unit)
    {
        if (unit is LengthUnit lu)
            return lu.ConvertToBaseUnit(value);

        if (unit is WeightUnit wu)
            return wu.ConvertToBaseUnit(value);

        if (unit is VolumeUnit vu)
            return vu.ConvertToBaseUnit(value);

        throw new ArgumentException("Unsupported unit type");
    }

    private double ConvertFromBase(double baseValue, U unit)
    {
        if (unit is LengthUnit lu)
            return lu.ConvertFromBaseUnit(baseValue);

        if (unit is WeightUnit wu)
            return wu.ConvertFromBaseUnit(baseValue);

        if (unit is VolumeUnit vu)
            return vu.ConvertFromBaseUnit(baseValue);

        throw new ArgumentException("Unsupported unit type");
    }
}