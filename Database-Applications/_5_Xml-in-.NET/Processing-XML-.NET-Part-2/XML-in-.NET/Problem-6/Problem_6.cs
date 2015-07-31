namespace XML_in_.NET_6_
{
    using System.Xml;

    public class Problem_6
    {
        public static void Main()
        {
            DeleteSaveAlbumsDoc();
        }

        private static void DeleteSaveAlbumsDoc()
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load("../../../catalog.xml");
            XmlNode rootNode = catalog.DocumentElement;

            foreach (XmlNode albumNode in rootNode.ChildNodes)
            {
                var albumPrice = decimal.Parse(albumNode["price"].InnerText);
                if (albumPrice > 20m)
                {
                    rootNode.RemoveChild(albumNode);
                }
            }

            catalog.Save("../../../cheap-albums-catalog.xml");
        }
    }
}
