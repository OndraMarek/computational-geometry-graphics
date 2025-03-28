class RaySphereIntersection
{
    static void Main()
    {
        double[] s = getUserInputs("S");
        double r = getUserInput("r");

        double[] a = getUserInputs("A");
        double[] q = getUserInputs("q");

        if (RayIntersectsSphere(s, r, a, q))
        {
            Console.WriteLine("\nRay INTERSECTS the sphere.\n");
        }
        else
        {
            Console.WriteLine("\nRay does NOT INTERSECT the sphere.\n");
        }
        exitOrContinue();
    }

    private static bool RayIntersectsSphere(double[] s, double r, double[] a, double[] q)
    {
        return false;
    }

    private static double[] getUserInputs(string element)
    {
        int size = 3;
        Console.WriteLine($"Enter the 3 coordinates for '{element}' separated by SPACES: ");
        string[] input = Console.ReadLine().Split(' ');

        if (input.Length != size)
        {
            Console.WriteLine("Input have to be 3 numbers. Please enter 3 coordinates separated by SPACES.");
            return getUserInputs(element);
        }
        double[] result = new double[size];
        for (int i = 0; i < size; i++)
        {
            if (!double.TryParse(input[i], out result[i]))
            {
                Console.WriteLine($"Invalid input '{input[i]}'. Please enter only numbers.");
                return getUserInputs(element);
            }
        }
        return result;
    }

    private static double getUserInput(string element)
    {
        Console.WriteLine($"Enter the radius '{element}' size: ");
        string input = Console.ReadLine();

        double result;
        if (!double.TryParse(input, out result))
        {
            Console.WriteLine($"Invalid input '{input}'. Please enter only numbers.");
            return getUserInput(element);
        }
        return result;
    }

    private static void exitOrContinue()
    {
        Console.WriteLine("Press 1 to continue or any other key to exit.");
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.KeyChar == '1')
        {
            Main();
        }
        else
        {
            Environment.Exit(0);
        }
    }
}