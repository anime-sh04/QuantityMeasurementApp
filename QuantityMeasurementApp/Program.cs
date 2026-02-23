namespace QuantityMeasurementApp;

// Entry point of the Quantity Measurement Application
public class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Quantity Measurement App");
        Controller menu = new();
        menu.Show();
    }
}