namespace Problem_8
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class Problem_8
    {
        public static void Main()
        {
            XDocument xmlDoc = XDocument.Load("../../../catalog.xml");

            var albums =
                from album in xmlDoc.Descendants("album")
                where double.Parse(album.Element("year").Value) <= 2010
                select new
                {
                    Name = album.Attribute("name").Value,
                    Price = album.Element("price").Value
                };

            foreach (var album in albums)
            {
                Console.WriteLine("Album name: {0} / price : {1}", album.Name, album.Price);
            }
        }
    }
}
