namespace QuantityMeasurementApp;
using QuantityMeasurementApp.ModelLayer;

public class QuantityService
{
    // -----------------------------
    // LENGTH EQUALITY (UC3)
    // -----------------------------
    public void DemonstrateQuantityEquality()
    {
        Console.WriteLine("Enter first quantity value:");
        double value1 = ReadDoubleInput();

        Console.WriteLine("Enter first unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit1 = ReadLengthUnitInput();

        Console.WriteLine("Enter second quantity value:");
        double value2 = ReadDoubleInput();

        Console.WriteLine("Enter second unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit2 = ReadLengthUnitInput();

        QuantityLength q1 = new QuantityLength(value1, unit1);
        QuantityLength q2 = new QuantityLength(value2, unit2);

        bool result = q1.Equals(q2);

        Console.WriteLine($"\nInput: Quantity({value1}, {unit1}) and Quantity({value2}, {unit2})");
        Console.WriteLine("Output: Equal (" + result + ")");
    }

    // -----------------------------
    // LENGTH ADDITION (UC6)
    // -----------------------------
    public void DemonstrateQuantityAddition()
    {
        Console.WriteLine("Enter first quantity value:");
        double value1 = ReadDoubleInput();

        Console.WriteLine("Enter first unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit1 = ReadLengthUnitInput();

        Console.WriteLine("Enter second quantity value:");
        double value2 = ReadDoubleInput();

        Console.WriteLine("Enter second unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit2 = ReadLengthUnitInput();

        QuantityLength q1 = new QuantityLength(value1, unit1);
        QuantityLength q2 = new QuantityLength(value2, unit2);

        QuantityLength result = QuantityLength.Add(q1, q2);

        Console.WriteLine($"\nInput: Quantity({value1}, {unit1}) + Quantity({value2}, {unit2})");
        Console.WriteLine("Output: " + result);
    }

    // -----------------------------
    // LENGTH ADDITION WITH TARGET UNIT (UC7)
    // -----------------------------
    public void DemonstrateQuantityAdditionWithTargetUnit()
    {
        Console.WriteLine("Enter first quantity value:");
        double value1 = ReadDoubleInput();

        Console.WriteLine("Enter first unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit1 = ReadLengthUnitInput();

        Console.WriteLine("Enter second quantity value:");
        double value2 = ReadDoubleInput();

        Console.WriteLine("Enter second unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit2 = ReadLengthUnitInput();

        Console.WriteLine("Enter target unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit targetUnit = ReadLengthUnitInput();

        QuantityLength q1 = new QuantityLength(value1, unit1);
        QuantityLength q2 = new QuantityLength(value2, unit2);

        QuantityLength result = QuantityLength.Add(q1, q2, targetUnit);

        Console.WriteLine($"\nInput: Quantity({value1}, {unit1}) + Quantity({value2}, {unit2})");
        Console.WriteLine($"Target Unit: {targetUnit}");
        Console.WriteLine($"Output: {result}");
    }

    // -----------------------------
    // UC9 WEIGHT EQUALITY
    // -----------------------------
    public void DemonstrateWeightEquality()
    {
        Console.WriteLine("Enter first weight value:");
        double value1 = ReadDoubleInput();

        Console.WriteLine("Enter first unit (Kilogram/Gram/Pound):");
        WeightUnit unit1 = ReadWeightUnitInput();

        Console.WriteLine("Enter second weight value:");
        double value2 = ReadDoubleInput();

        Console.WriteLine("Enter second unit (Kilogram/Gram/Pound):");
        WeightUnit unit2 = ReadWeightUnitInput();

        QuantityWeight w1 = new QuantityWeight(value1, unit1);
        QuantityWeight w2 = new QuantityWeight(value2, unit2);

        bool result = w1.Equals(w2);

        Console.WriteLine($"\nInput: Quantity({value1}, {unit1}) and Quantity({value2}, {unit2})");
        Console.WriteLine("Output: Equal (" + result + ")");
    }

    // -----------------------------
    // UC9 WEIGHT ADDITION
    // -----------------------------
    public void DemonstrateWeightAddition()
    {
        Console.WriteLine("Enter first weight value:");
        double value1 = ReadDoubleInput();

        Console.WriteLine("Enter first unit (Kilogram/Gram/Pound):");
        WeightUnit unit1 = ReadWeightUnitInput();

        Console.WriteLine("Enter second weight value:");
        double value2 = ReadDoubleInput();

        Console.WriteLine("Enter second unit (Kilogram/Gram/Pound):");
        WeightUnit unit2 = ReadWeightUnitInput();

        QuantityWeight w1 = new QuantityWeight(value1, unit1);
        QuantityWeight w2 = new QuantityWeight(value2, unit2);

        QuantityWeight result = QuantityWeight.Add(w1, w2);

        Console.WriteLine($"\nInput: Quantity({value1}, {unit1}) + Quantity({value2}, {unit2})");
        Console.WriteLine($"Output: {result}");
    }

    // -----------------------------
    // HELPER METHODS
    // -----------------------------

    private double ReadDoubleInput()
    {
        while (true)
        {
            string input = Console.ReadLine();

            if (double.TryParse(input, out double value))
                return value;

            Console.WriteLine("Invalid numeric input. Please enter a valid number:");
        }
    }

    private LengthUnit ReadLengthUnitInput()
    {
        while (true)
        {
            string input = Console.ReadLine();

            if (Enum.TryParse(input, true, out LengthUnit unit))
                return unit;

            Console.WriteLine("Invalid unit. Enter Feet/Inch/Yard/Centimeter:");
        }
    }

    private WeightUnit ReadWeightUnitInput()
    {
        while (true)
        {
            string input = Console.ReadLine();

            if (Enum.TryParse(input, true, out WeightUnit unit))
                return unit;

            Console.WriteLine("Invalid unit. Enter Kilogram/Gram/Pound:");
        }
    }
}