namespace DistanceCalculatorClient
{
    using System;
    using DistanceCalculatorReference;

    public class DistanceCalculatorClientMain
    {
        static void Main()
        {
            using (var calculatorClient = new CalculatorClient())
            {
                var result = calculatorClient.CalculateDistance(
                new Point { X = -2, Y = 6 },
                new Point { X = 11, Y = 100 }
            );

                Console.WriteLine("Service result: {0:F}", result);
            }
        }
    }
}
