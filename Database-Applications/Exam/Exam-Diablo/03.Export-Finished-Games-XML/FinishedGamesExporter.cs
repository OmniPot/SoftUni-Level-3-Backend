namespace _03.Export_Finished_Games_XML
{
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;
    using _01.EF_Mapping_Db_First;

    public class FinishedGamesExporter
    {
        public static void Main()
        {
            var filePath = @"..\..\..\";
            var fileName = @"finished-games.xml";

            var context = new DiabloEntities();
            var finishedGames = context.Games
                .Where(g => g.IsFinished)
                .OrderBy(g => g.Name)
                .ThenBy(g => g.Duration)
                .Select(g => new
                {
                    Name = g.Name,
                    Duration = g.Duration,
                    Users = g.UsersGames
                        .Select(ug => new
                        {
                            ug.User.Username,
                            ug.User.IpAddress
                        })
                });

            var xmlGamesRoot = new XElement("games");
            var xmlGames = new XDocument(xmlGamesRoot);

            foreach (var game in finishedGames)
            {
                var xmlGame = new XElement("game", new XAttribute("name", game.Name));
                var xmlUsers = new XElement("users");
                if (game.Duration != null)
                {
                    var durationAttribute = new XAttribute("duration", game.Duration);
                    xmlGame.Add(durationAttribute);
                }

                foreach (var user in game.Users)
                {
                    var xmlUser = new XElement("user",
                        new XAttribute("username", user.Username),
                        new XAttribute("ip-address", user.IpAddress));
                    xmlUsers.Add(xmlUser);
                }

                xmlGame.Add(xmlUsers);
                xmlGames.Root.Add(xmlGame);
            }

            xmlGames.Save(filePath + fileName);

            Process.Start(filePath);
        }
    }
}
