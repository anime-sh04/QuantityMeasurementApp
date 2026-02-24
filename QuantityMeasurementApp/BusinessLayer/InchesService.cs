
namespace QuantityMeasurementApp;
// Service class responsible for handling Inches equality operations
public class InchesServices
{
    public void CheckEquality()
    {
        Console.WriteLine("Enter 1st Value in Inches : ");
        double inchesValueOne = double.Parse(Console.ReadLine());
        Console.WriteLine("Enter 2nd Value in Inches : ");
        double inchesValueTwo = double.Parse(Console.ReadLine());

        Inches i1 = new Inches(inchesValueOne);
        Inches i2 = new Inches(inchesValueTwo);

        bool result = i1.Equals(i2);

        Console.Write("\n\nComparison Result : ");
        Console.WriteLine(result ? "true" : "false");
    }
}
