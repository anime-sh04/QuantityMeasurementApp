namespace QuantityMeasurementApp;

public class Controller
{
    private FeetServices feetServices = new FeetServices();
    private InchesServices inchServices = new InchesServices();
    public void Show()
    {
        while(true)
        {
            Console.WriteLine("1. Demonstrate Feet Equality");
            Console.WriteLine("2. Demonstrate Inch Equality");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1: feetServices.CheckEquality();break;
                case 2: inchServices.CheckEquality();break;
                default: Console.WriteLine("Invalid Choice");break;
            }
        }
    }
}