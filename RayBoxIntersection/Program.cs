class RayCuboidIntersection
{
    static void Main()
    {
        double[] b = getUserInput("b");
        double[] v = getUserInput("v");

        double[] a = getUserInput("a");
        double[] q = getUserInput("q");

        if (RayIntersectsCuboid(b,v,a,q))
        {
            Console.WriteLine("Ray intersects the cuboid.");
        }
        else
        {
            Console.WriteLine("Ray does not intersect the cuboid.");
        }
        exitOrContinue();
    }

    private static bool RayIntersectsCuboid(double[] b, double[] v, double[] a, double[] q)
    {
        double tn = 0.0;
        double tf = double.PositiveInfinity;

        for (int i = 0; i < 3; i++)
        {
            double tni = (b[i] - a[i]) / q[i];
            double tfi = (v[i] - a[i]) / q[i];

            if (tni > tfi)
            {
                (tni, tfi) = (tfi, tni);
            }

            tn = Math.Max(tn, tni);
            tf = Math.Min(tf, tfi);

            if (tn > tf)
            {
                return false;
            }
        }

        if (tn >= 0 && tf >= 0)
        {
            return true;
        }
        return false;
    }

    private static double[] getUserInput(string element)
    {
        Console.WriteLine("Enter the 3 elements for" + element + "separated by SPACES: ");
        string[] input = Console.ReadLine().Split(' ');
        if(input.Length != 3)
        {
            Console.WriteLine("Invalid input. Please enter 3 elements separated by SPACES.");
            return getUserInput(element);
        }
        double[] A = new double[3];
        for (int i = 0; i < input.Length; i++)
        {
            if (!double.TryParse(input[i], out A[i]))
            {
                Console.WriteLine($"Invalid input '{input[i]}'. Please enter only integers.");
                return getUserInput(element);
            }
        }
        return A;
    }

    private static void printOutput(int[] A, int[] S)
    {
        // Print the input array
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