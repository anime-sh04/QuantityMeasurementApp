namespace QuantityMeasurementApp;

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
            Console.WriteLine("3. Demonstrate Length Equality");
            Console.WriteLine("4. Demonstrate Length Addition");
            Console.WriteLine("5. Demonstrate Length Addition with Target Unit");
            Console.WriteLine("6. Demonstrate Weight Equality");
            Console.WriteLine("7. Demonstrate Weight Addition");
            Console.WriteLine("0. Exit");

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
                    quantityServices.DemonstrateQuantityEquality();
                    break;

                case 4:
                    quantityServices.DemonstrateQuantityAddition();
                    break;

                case 5:
                    quantityServices.DemonstrateQuantityAdditionWithTargetUnit();
                    break;

                case 6:
                    quantityServices.DemonstrateWeightEquality();
                    break;

                case 7:
                    quantityServices.DemonstrateWeightAddition();
                    break;

                default:
                    Console.WriteLine("Invalid Choice");
                    break;
            }
        }
    }
}