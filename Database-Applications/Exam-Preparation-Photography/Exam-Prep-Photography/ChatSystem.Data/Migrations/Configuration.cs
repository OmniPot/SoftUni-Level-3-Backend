namespace ChatSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Phonebook.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ChatSystemContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ChatSystemContext context)
        {
            SeedUsers(context);
            SeedChannels(context);
            SeedChannelMessages(context);
        }

        private void SeedChannelMessages(ChatSystemContext context)
        {
            if (!context.ChannelMessages.Any())
            {
                var channel = context.Channels.FirstOrDefault(c => c.Name.Equals("Malinki"));
                var contents = new[]
                {
                    "Hey dudes, are you ready for tonight?",
                    "Hey Petya, this is the SoftUni chat",
                    "Hahaha, we are ready!",
                    "Oh my god. I mean for drinking beers!",
                    "We are sure!"
                };
                var users = new User[]
                {
                    context.Users.FirstOrDefault(u => u.Username.Equals("Petya")),
                    context.Users.FirstOrDefault(u => u.Username.Equals("VGeorgiev")),
                    context.Users.FirstOrDefault(u => u.Username.Equals("Nakov")),
                    context.Users.FirstOrDefault(u => u.Username.Equals("Petya")),
                    context.Users.FirstOrDefault(u => u.Username.Equals("VGeorgiev"))
                };

                for (int i = 0; i < users.Length; i++)
                {
                    var cMessage = new ChannelMessage()
                    {
                        Channel = channel,
                        Content = contents[i],
                        Datetime = DateTime.Now,
                        User = users[i]
                    };

                    context.ChannelMessages.Add(cMessage);
                }

                context.SaveChanges();
            }
        }

        private void SeedChannels(ChatSystemContext context)
        {
            if (!context.Channels.Any())
            {
                var channelName = new[] { "Malinki", "SoftUni", "Admins", "Programmers", "Geeks" };
                foreach (var name in channelName)
                {
                    var channel = new Channel()
                    {
                        Name = name
                    };

                    context.Channels.Add(channel);
                }

                context.SaveChanges();
            }
        }

        private static void SeedUsers(ChatSystemContext context)
        {
            if (!context.Users.Any())
            {
                var usernames = new[] { "VGeorgiev", "Nakov", "Ache", "Alex", "Petya" };
                var fullNames = new[]
                {
                    "Vladimir Georgiev",
                    "Svetlin Nakov",
                    "Angel Georgiev",
                    "Alexandra Svilarova",
                    "Petya Grozdarska"
                };
                var phoneNumbers = new[]
                {
                    "0894545454",
                    "0897878787",
                    "0897121212",
                    "0894151417",
                    "0895464646"
                };

                for (int i = 0; i < usernames.Length; i++)
                {
                    var user = new User()
                    {
                        Username = usernames[i],
                        FullName = fullNames[i],
                        PhoneNumber = phoneNumbers[i]
                    };

                    context.Users.Add(user);
                }
            }

            context.SaveChanges();
        }
    }
}
