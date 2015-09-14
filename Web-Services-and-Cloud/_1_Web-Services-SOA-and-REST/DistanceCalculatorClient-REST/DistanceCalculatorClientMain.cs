namespace DistanceCalculatorClient_REST
{
    using System;
    using System.Net;

    public class DistanceCalculatorClientMain
    {
        public static void Main()
        {
            var requestUrl = "http://localhost:55538/api/points/distance?startX=5&startY=5&endX=10&endY=10";
            using (var client = new WebClient())
            {
                var response = client.UploadString(requestUrl, "POST", "");

                Console.WriteLine("Calculated distance: {0}", response);
            }
        }
    }
}
