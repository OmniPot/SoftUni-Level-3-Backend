namespace XML_in_.NET
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class Problems_1_to_5
    {
        public static void Main()
        {
            ExtractAlbumNames();

            ExtractAllArtists();

            ExtractArtistsAndNumberOfAlbums();

            ExtractArtistsAndNumberOfAlbumsWithXPath();
        }

        // Task 5
        private static void ExtractArtistsAndNumberOfAlbumsWithXPath()
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load("../../../catalog.xml");

            Dictionary<string, int> artistsAlbums = new Dictionary<string, int>();

            string xPathQuery = "/music/album";
            XmlNodeList albumList = catalog.SelectNodes(xPathQuery);

            foreach (XmlNode node in albumList)
            {
                string artistName = node.ChildNodes[0].Attributes["name"].Value;
                if (artistsAlbums.ContainsKey(artistName))
                {
                    artistsAlbums[artistName]++;
                }
                else
                {
                    artistsAlbums.Add(artistName, 1);
                }
            }

            Console.WriteLine("\nCount of albums for each artist using XPATH:");
            foreach (var artist in artistsAlbums)
            {
                Console.WriteLine("-- {0}: {1}", artist.Key, artist.Value);
            }
        }

        // Task 4
        private static void ExtractArtistsAndNumberOfAlbums()
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load("../../../catalog.xml");
            XmlNode rootNode = catalog.DocumentElement;

            Dictionary<string, int> artistsAlbums = new Dictionary<string, int>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                var artist = node.ChildNodes[0].Attributes["name"].Value;
                if (artistsAlbums.ContainsKey(artist))
                {
                    artistsAlbums[artist]++;
                }
                else
                {
                    artistsAlbums.Add(artist, 1);
                }
            }

            Console.WriteLine("\nCount of albums for each artist:");
            foreach (var artist in artistsAlbums)
            {
                Console.WriteLine("-- {0}: {1}", artist.Key, artist.Value);
            }
        }

        // Task 3
        private static void ExtractAllArtists()
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load("../../../catalog.xml");
            XmlNode rootNode = catalog.DocumentElement;

            SortedSet<string> artists = new SortedSet<string>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                artists.Add(node.ChildNodes[0].Attributes["name"].Value);
            }

            Console.WriteLine("\nArtists: \n--{0}", String.Join("\n--", artists));
        }

        // Task 2
        private static void ExtractAlbumNames()
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load("../../../catalog.xml");
            XmlNode rootNode = catalog.DocumentElement;

            Console.WriteLine("Album names:");
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                Console.WriteLine("-- {0}", node.Attributes["name"].Value);
            }
        }
    }
}
