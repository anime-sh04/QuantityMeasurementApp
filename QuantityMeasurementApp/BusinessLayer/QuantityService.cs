
namespace QuantityMeasurementApp;
using QuantityMeasurementApp.ModelLayer;
// Service class for UC3: Generic Quantity equality using DRY principle
public class QuantityService
{
    public void DemonstrateQuantityEquality()
    {
        // Take first quantity input
        Console.WriteLine("Enter first quantity value:");
        double value1 = ReadDoubleInput();

        Console.WriteLine("Enter first unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit1 = ReadUnitInput();

        // Take second quantity input
        Console.WriteLine("Enter second quantity value:");
        double value2 = ReadDoubleInput();

        Console.WriteLine("Enter second unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit2 = ReadUnitInput();

        // Create QuantityLength objects (Encapsulation of value + unit)
        QuantityLength q1 = new QuantityLength(value1, unit1);
        QuantityLength q2 = new QuantityLength(value2, unit2);

        // Perform cross-unit equality comparison (UC3 core logic)
        bool result = q1.Equals(q2);

        // Display result
        Console.WriteLine($"\nInput: Quantity({value1}, {unit1}) and Quantity({value2}, {unit2})");
        Console.WriteLine("Output: Equal (" + result + ")");
    }

    public void DemonstrateQuantityAddition()
    {
        // First length input
        Console.WriteLine("Enter first quantity value:");
        double value1 = ReadDoubleInput();
    
        Console.WriteLine("Enter first unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit1 = ReadUnitInput();
    
        // Second length input
        Console.WriteLine("Enter second quantity value:");
        double value2 = ReadDoubleInput();
    
        Console.WriteLine("Enter second unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit2 = ReadUnitInput();
    
        // Create QuantityLength objects
        QuantityLength q1 = new QuantityLength(value1, unit1);
        QuantityLength q2 = new QuantityLength(value2, unit2);
    
        // Perform addition (UC6)
        QuantityLength result = QuantityLength.Add(q1,q2);
    
        Console.WriteLine($"\nInput: Quantity({value1}, {unit1}) + Quantity({value2}, {unit2})");
        Console.WriteLine("Output: " + result);
    }

    public void DemonstrateQuantityAdditionWithTargetUnit()
    {
        Console.WriteLine("Enter first quantity value:");
        double value1 = ReadDoubleInput();

        Console.WriteLine("Enter first unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit1 = ReadUnitInput();

        Console.WriteLine("Enter second quantity value:");
        double value2 = ReadDoubleInput();

        Console.WriteLine("Enter second unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit unit2 = ReadUnitInput();

        Console.WriteLine("Enter target unit (Feet/Inch/Yard/Centimeter):");
        LengthUnit targetUnit = ReadUnitInput();

        QuantityLength q1 = new QuantityLength(value1, unit1);
        QuantityLength q2 = new QuantityLength(value2, unit2);

        QuantityLength result = QuantityLength.Add(q1, q2, targetUnit);

        Console.WriteLine($"\nInput: Quantity({value1}, {unit1}) + Quantity({value2}, {unit2})");
        Console.WriteLine($"Target Unit: {targetUnit}");
        Console.WriteLine($"Output: {result}");
    }

    // Helper method to safely read numeric input (UC3)
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

    // Helper method to validate and read unit input
    private LengthUnit ReadUnitInput()
    {
        while (true)
        {
            string input = Console.ReadLine();

            if (Enum.TryParse(input, true, out LengthUnit unit))
                return unit;

            Console.WriteLine("Invalid unit. Please enter either 'Feet'/'Inch'/'Yard'/'Centimeter':");
        }
    }
}