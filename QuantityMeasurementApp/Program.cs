namespace QuantityMeasurementApp;

// Entry point of the Quantity Measurement Application
public class Program
{
    static void Main()
    {
        Console.WriteLine("\n=========Welcome to Quantity Measurement App=========\n");
        Controller menu = new();
        menu.Show();
    }
}