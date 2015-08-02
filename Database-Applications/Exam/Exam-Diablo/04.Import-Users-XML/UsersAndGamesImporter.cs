namespace _04.Import_Users_XML
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using _01.EF_Mapping_Db_First;

    public class UsersAndGamesImporter
    {
        public static void Main()
        {
            var dataFile = @"..\..\users-and-games.xml";
            var context = new DiabloEntities();
            var xmlDoc = XDocument.Load(dataFile);

            foreach (var xmlUser in xmlDoc.Root.Descendants("user"))
            {
                var userName = xmlUser.Attribute("username").Value;
                var existingUser = context.Users.FirstOrDefault(u => u.Username.Equals(userName));

                if (existingUser == null)
                {
                    AddUserGame(xmlUser, context);
                }
                else
                {
                    Console.WriteLine("User {0} already exists", userName);
                }

                context.SaveChanges();
            }
        }

        private static void AddUserGame(XElement xmlUser, DiabloEntities context)
        {
            var newUser = CreateUserToAdd(xmlUser);
            context.Users.Add(newUser);
            Console.WriteLine("Successfully added user {0}", newUser.Username);

            foreach (var xmlGame in xmlUser.Descendants("game"))
            {
                var gameName = xmlGame.Element("game-name").Value;
                var characterName = xmlGame.Element("character").Attribute("name").Value;

                var existingGame = context.Games.First(g => g.Name.Equals(gameName));
                var character = context.Characters.First(ch => ch.Name == characterName);

                var gameCash = decimal.Parse(xmlGame.Element("character").Attribute("cash").Value);
                var level = int.Parse(xmlGame.Element("character").Attribute("level").Value);
                var joinedOn = DateTime.Parse(xmlGame.Element("joined-on").Value);

                var userGame = new UsersGame()
                {
                    CharacterId = character.Id,
                    GameId = existingGame.Id,
                    Cash = gameCash,
                    JoinedOn = joinedOn,
                    Level = level,
                    User = newUser
                };

                context.UsersGames.Add(userGame);
                Console.WriteLine("User {0} successfully added to game {1}", newUser.Username, existingGame.Name);
            }
        }

        private static User CreateUserToAdd(XElement xmlUser)
        {
            var userToAdd = new User()
            {
                Username = xmlUser.Attribute("username").Value,
                IsDeleted = xmlUser.Attribute("is-deleted").Value.Equals("1"),
                RegistrationDate = DateTime.Parse(xmlUser.Attribute("registration-date").Value),
                IpAddress = xmlUser.Attribute("ip-address").Value
            };
            var firstName = xmlUser.Attribute("first-name");
            var lastName = xmlUser.Attribute("last-name");
            var email = xmlUser.Attribute("email");

            AddOptionalUserProperties(firstName, userToAdd, lastName, email);
            return userToAdd;
        }

        private static void AddOptionalUserProperties(XAttribute firstName, User userToAdd, XAttribute lastName,
            XAttribute email)
        {
            if (firstName != null)
            {
                userToAdd.FirstName = firstName.Value;
            }
            if (lastName != null)
            {
                userToAdd.FirstName = lastName.Value;
            }
            if (email != null)
            {
                userToAdd.FirstName = email.Value;
            }
        }
    }
}
