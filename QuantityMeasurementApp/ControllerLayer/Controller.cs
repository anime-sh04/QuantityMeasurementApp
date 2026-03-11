namespace QuantityMeasurementApp;
using QuantityMeasurementApp.ModelLayer;

public class Controller
{
    private FeetServices feetServices = new FeetServices();
    private InchesServices inchServices = new InchesServices();
    private QuantityService quantityServices = new QuantityService();

    public void Show()
    {
        while (true)
        {
            Console.WriteLine("\n==== Quantity Measurement Menu ====");
            Console.WriteLine("1. Demonstrate Feet Equality");
            Console.WriteLine("2. Demonstrate Inch Equality");
            Console.WriteLine("3. Demonstrate Generic Equality");
            Console.WriteLine("4. Demonstrate Generic Addition");
            Console.WriteLine("5. Demonstrate Generic Addition With Target Unit");
            Console.WriteLine("6. Demonstrate Generic Subtraction");
            Console.WriteLine("7. Demonstrate Generic Subtraction With Target Unit");
            Console.WriteLine("8. Demonstrate Generic Division");
            Console.WriteLine("0. Exit");

            Console.Write("Enter choice: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 0)
            {
                Console.WriteLine("\n========= Thank you for using Quantity Measurement App =========\n");
                return;
            }

            switch (choice)
            {
                case 1:
                    feetServices.CheckEquality();
                    break;

                case 2:
                    inchServices.CheckEquality();
                    break;

                case 3:
                    quantityServices.DemonstrateEquality();
                    break;

                case 4:
                    quantityServices.DemonstrateAddition();
                    break;

                case 5:
                    quantityServices.DemonstrateAdditionWithTarget();
                    break;

                case 6:
                    quantityServices.DemonstrateSubtraction();
                    break;

                case 7:
                    quantityServices.DemonstrateSubtractionWithTarget();
                    break;

                case 8:
                    quantityServices.DemonstrateDivision();
                    break;

                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }
        }
    }
}