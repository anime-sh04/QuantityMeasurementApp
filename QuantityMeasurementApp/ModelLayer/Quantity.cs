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

    // ============================
    // ENUM FOR OPERATIONS (UC13)
    // ============================

    private enum ArithmeticOperation
    {
        ADD,
        SUBTRACT,
        DIVIDE
    }

    // ============================
    // ADD
    // ============================

    public Quantity<U> Add(Quantity<U> other)
    {
        return Add(other, this.unit);
    }

    public Quantity<U> Add(Quantity<U> other, U targetUnit)
    {
        validateArithmeticOperands(other, targetUnit, true);

        double resultBase = performBaseArithmetic(other, ArithmeticOperation.ADD);

        double result = ConvertFromBase(resultBase, targetUnit);

        return new Quantity<U>(Round(result), targetUnit);
    }

    // ============================
    // SUBTRACT
    // ============================

    public Quantity<U> Subtract(Quantity<U> other)
    {
        return Subtract(other, this.unit);
    }

    public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
    {
        validateArithmeticOperands(other, targetUnit, true);

        double resultBase = performBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

        double result = ConvertFromBase(resultBase, targetUnit);

        return new Quantity<U>(Round(result), targetUnit);
    }

    // ============================
    // DIVIDE
    // ============================

    public double Divide(Quantity<U> other)
    {
        validateArithmeticOperands(other, default(U), false);

        double result = performBaseArithmetic(other, ArithmeticOperation.DIVIDE);

        return result;
    }

    // ============================
    // CENTRALIZED VALIDATION
    // ============================

    private void validateArithmeticOperands(Quantity<U> other, U targetUnit, bool targetUnitRequired)
    {
        if (other == null)
            throw new ArgumentException("Operand cannot be null");

        if (targetUnitRequired && targetUnit == null)
            throw new ArgumentException("Target unit cannot be null");

        if (unit.GetType() != other.unit.GetType())
            throw new ArgumentException("Incompatible measurement categories");

        if (double.IsNaN(value) || double.IsInfinity(value) ||
            double.IsNaN(other.value) || double.IsInfinity(other.value))
            throw new ArgumentException("Values must be finite numbers");
    }

    // ============================
    // CORE ARITHMETIC HELPER
    // ============================

    private double performBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
    {
        double base1 = ConvertToBase(this.value, this.unit);
        double base2 = ConvertToBase(other.value, other.unit);

        switch (operation)
        {
            case ArithmeticOperation.ADD:
                return base1 + base2;

            case ArithmeticOperation.SUBTRACT:
                return base1 - base2;

            case ArithmeticOperation.DIVIDE:

                if (base2 == 0)
                    throw new ArithmeticException("Division by zero");

                return base1 / base2;

            default:
                throw new ArgumentException("Unsupported operation");
        }
    }

    // ============================
    // CONVERSION
    // ============================

    public Quantity<U> ConvertTo(U targetUnit)
    {
        double baseValue = ConvertToBase(value, unit);
        double converted = ConvertFromBase(baseValue, targetUnit);

        return new Quantity<U>(Round(converted), targetUnit);
    }

    // ============================
    // EQUALITY
    // ============================

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

    // ============================
    // HELPERS
    // ============================

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

    private double Round(double value)
    {
        return Math.Round(value, 2);
    }
}