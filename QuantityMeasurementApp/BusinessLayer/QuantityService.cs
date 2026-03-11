namespace QuantityMeasurementApp;
using QuantityMeasurementApp.ModelLayer;

public class QuantityService
{
    // -----------------------------
    // GENERIC EQUALITY (Length / Weight)
    // -----------------------------
    public void DemonstrateEquality()
    {
        Console.WriteLine("Select category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            double v1 = ReadDoubleInput();
            LengthUnit u1 = ReadLengthUnitInput();

            double v2 = ReadDoubleInput();
            LengthUnit u2 = ReadLengthUnitInput();

            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            Console.WriteLine(q1.Equals(q2));
        }
        else
        {
            double v1 = ReadDoubleInput();
            WeightUnit u1 = ReadWeightUnitInput();

            double v2 = ReadDoubleInput();
            WeightUnit u2 = ReadWeightUnitInput();

            var q1 = new Quantity<WeightUnit>(v1, u1);
            var q2 = new Quantity<WeightUnit>(v2, u2);

            Console.WriteLine(q1.Equals(q2));
        }
    }

    // -----------------------------
    // GENERIC ADDITION
    // -----------------------------
    public void DemonstrateAddition()
    {
        Console.WriteLine("Select category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            double v1 = ReadDoubleInput();
            LengthUnit u1 = ReadLengthUnitInput();

            double v2 = ReadDoubleInput();
            LengthUnit u2 = ReadLengthUnitInput();

            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            var result = q1.Add(q2);

            Console.WriteLine(result);
        }
        else
        {
            double v1 = ReadDoubleInput();
            WeightUnit u1 = ReadWeightUnitInput();

            double v2 = ReadDoubleInput();
            WeightUnit u2 = ReadWeightUnitInput();

            var q1 = new Quantity<WeightUnit>(v1, u1);
            var q2 = new Quantity<WeightUnit>(v2, u2);

            var result = q1.Add(q2);

            Console.WriteLine(result);
        }
    }

    // -----------------------------
    // GENERIC ADDITION WITH TARGET
    // -----------------------------
    public void DemonstrateAdditionWithTarget()
    {
        Console.WriteLine("Select category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            double v1 = ReadDoubleInput();
            LengthUnit u1 = ReadLengthUnitInput();

            double v2 = ReadDoubleInput();
            LengthUnit u2 = ReadLengthUnitInput();

            LengthUnit target = ReadLengthUnitInput();

            var q1 = new Quantity<LengthUnit>(v1, u1);
            var q2 = new Quantity<LengthUnit>(v2, u2);

            var result = q1.Add(q2, target);

            Console.WriteLine(result);
        }
        else
        {
            double v1 = ReadDoubleInput();
            WeightUnit u1 = ReadWeightUnitInput();

            double v2 = ReadDoubleInput();
            WeightUnit u2 = ReadWeightUnitInput();

            WeightUnit target = ReadWeightUnitInput();

            var q1 = new Quantity<WeightUnit>(v1, u1);
            var q2 = new Quantity<WeightUnit>(v2, u2);

            var result = q1.Add(q2, target);

            Console.WriteLine(result);
        }
    }

    // -----------------------------
    // HELPERS
    // -----------------------------

    private double ReadDoubleInput()
    {
        while (true)
        {
            Console.WriteLine("Enter Values : ");
            string input = Console.ReadLine();

            if (double.TryParse(input, out double value))
                return value;

            Console.WriteLine("Invalid number. Try again:");
        }
    }

    private LengthUnit ReadLengthUnitInput()
    {
        while (true)
        {
            Console.WriteLine("Enter unit (Feet/Inch/Yard/Centimeter):");

            string input = Console.ReadLine();

            if (Enum.TryParse(input, true, out LengthUnit unit))
                return unit;

            Console.WriteLine("Invalid length unit.");
        }
    }

    private WeightUnit ReadWeightUnitInput()
    {
        while (true)
        {
            Console.WriteLine("Enter unit (Kilogram/Gram/Pound):");

            string input = Console.ReadLine();

            if (Enum.TryParse(input, true, out WeightUnit unit))
                return unit;

            Console.WriteLine("Invalid weight unit.");
        }
    }
}