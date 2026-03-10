namespace QuantityMeasurementApp.ModelLayer;

// UC8: Refactored QuantityLength class
// Conversion responsibility moved to LengthUnitExtension

public class QuantityLength
{
    private readonly double value;
    private readonly LengthUnit unit;

    public QuantityLength(double value, LengthUnit unit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid numeric value");

        this.value = value;
        this.unit = unit;
    }

    // =========================
    // UC5 STATIC CONVERSION API
    // =========================
    public static double Convert(double value, LengthUnit source, LengthUnit target)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Value must be finite");

        if (source == target)
            return value;

        double baseValue = source.ConvertToBaseUnit(value);
        return target.ConvertFromBaseUnit(baseValue);
    }

    // =========================
    // UC5 INSTANCE CONVERSION
    // =========================
    public double ConvertTo(LengthUnit targetUnit)
    {
        double baseValue = unit.ConvertToBaseUnit(value);
        return targetUnit.ConvertFromBaseUnit(baseValue);
    }

    // =========================
    // UC6 ADDITION (result in first unit)
    // =========================
    public static QuantityLength Add(QuantityLength length1, QuantityLength length2)
    {
        if (length1 == null || length2 == null)
            throw new ArgumentException("Length operands cannot be null");

        double l1Feet = length1.unit.ConvertToBaseUnit(length1.value);
        double l2Feet = length2.unit.ConvertToBaseUnit(length2.value);

        double sumFeet = l1Feet + l2Feet;

        double resultValue = length1.unit.ConvertFromBaseUnit(sumFeet);

        return new QuantityLength(resultValue, length1.unit);
    }

    // =========================
    // UC7 ADDITION WITH TARGET UNIT
    // =========================
    public static QuantityLength Add(QuantityLength length1, QuantityLength length2, LengthUnit targetUnit)
    {
        if (length1 == null || length2 == null)
            throw new ArgumentException("Length operands cannot be null");

        double l1Feet = length1.unit.ConvertToBaseUnit(length1.value);
        double l2Feet = length2.unit.ConvertToBaseUnit(length2.value);

        double sumFeet = l1Feet + l2Feet;

        double resultValue = targetUnit.ConvertFromBaseUnit(sumFeet);

        return new QuantityLength(resultValue, targetUnit);
    }

    // =========================
    // EQUALITY CHECK
    // =========================
    public override bool Equals(object? obj)
    {
        if (this == obj)
            return true;

        if (obj == null || obj.GetType() != typeof(QuantityLength))
            return false;

        QuantityLength other = (QuantityLength)obj;

        double thisBase = unit.ConvertToBaseUnit(value);
        double otherBase = other.unit.ConvertToBaseUnit(other.value);

        const double EPSILON = 1e-6;
        return Math.Abs(thisBase - otherBase) < EPSILON;
    }

    // =========================
    // HASHCODE
    // =========================
    public override int GetHashCode()
    {
        return unit.ConvertToBaseUnit(value).GetHashCode();
    }

    // =========================
    // STRING DISPLAY
    // =========================
    public override string ToString()
    {
        return $"{value} {unit}";
    }
}