namespace QuantityMeasurementApp;

public class Controller
{
    private FeetServices feetServices = new FeetServices();
    private InchesServices inchServices = new InchesServices();
    private QuantityService quantityServices = new QuantityService();
    public void Show()
    {
        while(true)
        {
            Console.WriteLine("1. Demonstrate Feet Equality");
            Console.WriteLine("2. Demonstrate Inch Equality");
            Console.WriteLine("3. Demonstrate Quantity Equality");
            Console.WriteLine("4. Demonstrate Quantity Addition");
            Console.WriteLine("0. Exit");
    
            int choice = int.Parse(Console.ReadLine());
    
            if (choice == 0){
                Console.WriteLine("\n=========Thankyou for using Quantity Measurement App=========\n");
                return;
            }
    
            switch (choice)
            {
                case 1: feetServices.CheckEquality(); break;
                case 2: inchServices.CheckEquality(); break;
                case 3: quantityServices.DemonstrateQuantityEquality(); break;
                case 4: quantityServices.DemonstrateQuantityAddition(); break;
                default: Console.WriteLine("Invalid Choice"); break;
            }
        }
    }
}
