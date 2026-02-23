namespace QuantityMeasurementApp;

// Service class responsible for handling Feet equality operations
public class FeetServices
{
    public void CheckEquality()
    {
        Console.WriteLine("Enter 1st Value in Feet : ");
        double feetValueOne = double.Parse(Console.ReadLine());
        Console.WriteLine("Enter 2nd Value in Feet : ");
        double feetValueTwo = double.Parse(Console.ReadLine());

        Feet f1 = new Feet(feetValueOne);
        Feet f2 = new Feet(feetValueTwo);

        bool result = f1.Equals(f2);

        Console.Write("\n\nComparison Result : ");
        Console.WriteLine(result ? "true" : "false");
    }
}
