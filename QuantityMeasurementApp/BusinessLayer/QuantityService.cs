namespace QuantityMeasurementApp;
using QuantityMeasurementApp.ModelLayer;

public class QuantityService
{
    // -----------------------------
    // GENERIC EQUALITY
    // -----------------------------
    public void DemonstrateEquality()
    {
        Console.WriteLine("Select category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");
        Console.WriteLine("3. Volume");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            var q1 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());
            var q2 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());

            Console.WriteLine(q1.Equals(q2));
        }
        else if (choice == 2)
        {
            var q1 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());
            var q2 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());

            Console.WriteLine(q1.Equals(q2));
        }
        else
        {
            var q1 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());
            var q2 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());

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
        Console.WriteLine("3. Volume");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            var q1 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());
            var q2 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());

            Console.WriteLine(q1.Add(q2));
        }
        else if (choice == 2)
        {
            var q1 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());
            var q2 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());

            Console.WriteLine(q1.Add(q2));
        }
        else
        {
            var q1 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());
            var q2 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());

            Console.WriteLine(q1.Add(q2));
        }
    }

    // -----------------------------
    // ADDITION WITH TARGET
    // -----------------------------
    public void DemonstrateAdditionWithTarget()
    {
        Console.WriteLine("Select category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");
        Console.WriteLine("3. Volume");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            var q1 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());
            var q2 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());

            LengthUnit target = ReadLengthUnitInput();

            Console.WriteLine(q1.Add(q2, target));
        }
        else if (choice == 2)
        {
            var q1 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());
            var q2 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());

            WeightUnit target = ReadWeightUnitInput();

            Console.WriteLine(q1.Add(q2, target));
        }
        else
        {
            var q1 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());
            var q2 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());

            VolumeUnit target = ReadVolumeUnitInput();

            Console.WriteLine(q1.Add(q2, target));
        }
    }

    // -----------------------------
    // SUBTRACTION (UC12)
    // -----------------------------
    public void DemonstrateSubtraction()
    {
        Console.WriteLine("Select category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");
        Console.WriteLine("3. Volume");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            var q1 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());
            var q2 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());

            Console.WriteLine(q1.Subtract(q2));
        }
        else if (choice == 2)
        {
            var q1 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());
            var q2 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());

            Console.WriteLine(q1.Subtract(q2));
        }
        else
        {
            var q1 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());
            var q2 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());

            Console.WriteLine(q1.Subtract(q2));
        }
    }

    // -----------------------------
    // SUBTRACTION WITH TARGET
    // -----------------------------
    public void DemonstrateSubtractionWithTarget()
    {
        Console.WriteLine("Select category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");
        Console.WriteLine("3. Volume");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            var q1 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());
            var q2 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());

            LengthUnit target = ReadLengthUnitInput();

            Console.WriteLine(q1.Subtract(q2, target));
        }
        else if (choice == 2)
        {
            var q1 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());
            var q2 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());

            WeightUnit target = ReadWeightUnitInput();

            Console.WriteLine(q1.Subtract(q2, target));
        }
        else
        {
            var q1 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());
            var q2 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());

            VolumeUnit target = ReadVolumeUnitInput();

            Console.WriteLine(q1.Subtract(q2, target));
        }
    }

    // -----------------------------
    // DIVISION (UC12)
    // -----------------------------
    public void DemonstrateDivision()
    {
        Console.WriteLine("Select category:");
        Console.WriteLine("1. Length");
        Console.WriteLine("2. Weight");
        Console.WriteLine("3. Volume");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            var q1 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());
            var q2 = new Quantity<LengthUnit>(ReadDoubleInput(), ReadLengthUnitInput());

            Console.WriteLine(q1.Divide(q2));
        }
        else if (choice == 2)
        {
            var q1 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());
            var q2 = new Quantity<WeightUnit>(ReadDoubleInput(), ReadWeightUnitInput());

            Console.WriteLine(q1.Divide(q2));
        }
        else
        {
            var q1 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());
            var q2 = new Quantity<VolumeUnit>(ReadDoubleInput(), ReadVolumeUnitInput());

            Console.WriteLine(q1.Divide(q2));
        }
    }

    // -----------------------------
    // HELPERS
    // -----------------------------

    private double ReadDoubleInput()
    {
        while (true)
        {
            Console.WriteLine("Enter Value:");
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

            if (Enum.TryParse(Console.ReadLine(), true, out LengthUnit unit))
                return unit;

            Console.WriteLine("Invalid length unit.");
        }
    }

    private WeightUnit ReadWeightUnitInput()
    {
        while (true)
        {
            Console.WriteLine("Enter unit (Kilogram/Gram/Pound):");

            if (Enum.TryParse(Console.ReadLine(), true, out WeightUnit unit))
                return unit;

            Console.WriteLine("Invalid weight unit.");
        }
    }

    private VolumeUnit ReadVolumeUnitInput()
    {
        while (true)
        {
            Console.WriteLine("Enter unit (Litre/Millilitre/Gallon):");

            if (Enum.TryParse(Console.ReadLine(), true, out VolumeUnit unit))
                return unit;

            Console.WriteLine("Invalid volume unit.");
        }
    }
}