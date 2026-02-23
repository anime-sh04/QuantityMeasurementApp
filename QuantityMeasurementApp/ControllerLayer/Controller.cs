namespace QuantityMeasurementApp;

public class Controller
{
    private FeetServices feetServices = new FeetServices();
    public void Show()
    {
        while(true)
        {
            Console.WriteLine("1. Demonstrate Feet Equality");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1: feetServices.CheckEquality();break;
            }
        }
    }
}