
namespace QuantityMeasurementApp;
// Service class for UC3: Generic Quantity equality using DRY principle
public class QuantityService
{
    public void DemonstrateQuantityEquality()
    {
        // Take first quantity input
        Console.WriteLine("Enter first quantity value:");
        double value1 = ReadDoubleInput();

        Console.WriteLine("Enter first unit (Feet/Inch):");
        LengthUnit unit1 = ReadUnitInput();

        // Take second quantity input
        Console.WriteLine("Enter second quantity value:");
        double value2 = ReadDoubleInput();

        Console.WriteLine("Enter second unit (Feet/Inch):");
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

    // Helper method to safely read numeric input (Validation as per UC3)
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

    // Helper method to validate and read unit input (Enum Safety)
    private LengthUnit ReadUnitInput()
    {
        while (true)
        {
            string input = Console.ReadLine();

            if (Enum.TryParse(input, true, out LengthUnit unit))
                return unit;

            Console.WriteLine("Invalid unit. Please enter either 'Feet' or 'Inch':");
        }
    }
}