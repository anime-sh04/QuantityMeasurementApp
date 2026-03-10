namespace QuantityMeasurementApp.ModelLayer;

public class QuantityWeight
{
    private readonly double value;
    private readonly WeightUnit unit;

    public QuantityWeight(double value, WeightUnit unit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid numeric value");

        this.value = value;
        this.unit = unit;
    }

    // ======================
    // Convert weight
    // ======================
    public QuantityWeight ConvertTo(WeightUnit targetUnit)
    {
        double baseValue = unit.ConvertToBaseUnit(value);
        double converted = targetUnit.ConvertFromBaseUnit(baseValue);

        return new QuantityWeight(converted, targetUnit);
    }

    // ======================
    // Add (result in first unit)
    // ======================
    public static QuantityWeight Add(QuantityWeight w1, QuantityWeight w2)
    {
        if (w1 == null || w2 == null)
            throw new ArgumentException("Weight operands cannot be null");

        double base1 = w1.unit.ConvertToBaseUnit(w1.value);
        double base2 = w2.unit.ConvertToBaseUnit(w2.value);

        double sum = base1 + base2;

        double result = w1.unit.ConvertFromBaseUnit(sum);

        return new QuantityWeight(result, w1.unit);
    }

    // ======================
    // Add with target unit
    // ======================
    public static QuantityWeight Add(QuantityWeight w1, QuantityWeight w2, WeightUnit targetUnit)
    {
        if (w1 == null || w2 == null)
            throw new ArgumentException("Weight operands cannot be null");

        double base1 = w1.unit.ConvertToBaseUnit(w1.value);
        double base2 = w2.unit.ConvertToBaseUnit(w2.value);

        double sum = base1 + base2;

        double result = targetUnit.ConvertFromBaseUnit(sum);

        return new QuantityWeight(result, targetUnit);
    }
    public double GetValue()
    {
        return value;
    }
    // ======================
    // Equality check
    // ======================
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(QuantityWeight))
            return false;

        QuantityWeight other = (QuantityWeight)obj;

        double thisBase = unit.ConvertToBaseUnit(value);
        double otherBase = other.unit.ConvertToBaseUnit(other.value);

        const double EPSILON = 1e-6;
        return Math.Abs(thisBase - otherBase) < EPSILON;
    }

    public override int GetHashCode()
    {
        return unit.ConvertToBaseUnit(value).GetHashCode();
    }

    public override string ToString()
    {
        return $"{value} {unit}";
    }
}