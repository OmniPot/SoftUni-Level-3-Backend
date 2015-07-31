namespace Problem_7
{
    using System;
    using System.Xml;

    public class Problem_7
    {
        public static void Main()
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load("../../../catalog.xml");
            XmlNode rootNode = catalog.DocumentElement;

            string xPathQuery = "/music/album[year<=2010]/@name | /music/album[year<=2010]/price";
            XmlNodeList queryResult = catalog.SelectNodes(xPathQuery);

            Console.WriteLine("Albums before 2010: ");
            foreach (XmlNode result in queryResult)
            {
                Console.WriteLine("-- {0}", result.InnerText);
            }
        }
    }
}
