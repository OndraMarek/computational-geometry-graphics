class RaySphereIntersection
{
    static void Main()
    {
        double[] S = getUserInputs("S");
        double r = getUserInput("r");

        double[] A = getUserInputs("A");
        double[] q = getUserInputs("q");

        double[]? intersection = RayIntersectsSphere(S, r, A, q);

        if (intersection != null)
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("\nRay INTERSECTS the sphere.");
            Console.WriteLine($"Intersection point coordinates: ({intersection[0]}, {intersection[1]}, {intersection[2]})\n");
            Console.WriteLine("-----------------------------------");
        }
        else
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("\nRay does NOT INTERSECT the sphere.\n");
            Console.WriteLine("-----------------------------------");
        }
        exitOrContinue();
    }

    private static double[]? RayIntersectsSphere(double[] S, double r, double[] A, double[] q)
    {
        double[] p = new double[3];
        for (int i = 0; i < 3; i++)
            p[i] = A[i] - S[i];

        double a = ScalarProduct(q, q);
        double b = 2 * ScalarProduct(p, q);
        double c = ScalarProduct(p, p) - r * r;

        double D = b * b - 4 * a * c;

        if (D < 0)
        {
            return null;
        }

        double sqrtD = Math.Sqrt(D);
        double t1 = (-b + sqrtD) / (2 * a);
        double t2 = (-b - sqrtD) / (2 * a);

        double[] intersection = new double[3];

        if (t1 < 0 && t2 < 0)
        {
            return null;
        }

        if (t1 > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                intersection[i] = A[i] + t1 * q[i];
            }
            return intersection;
        }

        else
        {
            for (int i = 0; i < 3; i++)
            {
                intersection[i] = A[i] + t2 * q[i];
            }
            return intersection;
        }
    }

    private static double ScalarProduct(double[] v1, double[] v2)
    {
        return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
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

        if (input.Split(' ').Length > 1)
        {
            Console.WriteLine($"Invalid input '{input}'. Please enter only one number.");
            return getUserInput(element);
        }

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
            Console.Clear();
            Main();
        }
        else
        {
            Environment.Exit(0);
        }
    }
}