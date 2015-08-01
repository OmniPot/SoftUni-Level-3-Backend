namespace ChatSystem.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Data;
    using Models;
    using Newtonsoft.Json;
    using Phonebook.Models;

    public class ChatSystemMain
    {
        public static void Main()
        {
            var context = new ChatSystemContext();

            TestSeedMethod(context);
            ImportUserMessagesFromJson(context);
        }

        private static void ImportUserMessagesFromJson(ChatSystemContext context)
        {
            var filePath = "../../messages.json";
            var fileTextContent = File.ReadAllText(filePath);

            var userMessages = JsonConvert.DeserializeObject<List<JsonUserMessage>>(fileTextContent);
            foreach (var msg in userMessages)
            {
                if (msg.Content == null || msg.DateTime == null || msg.Sender == null || msg.Recipient == null)
                {
                    Console.WriteLine("All fields are required!");
                }
                else
                {
                    var datetime = msg.DateTime;
                    var sender = context.Users.FirstOrDefault(u => u.Username.Equals(msg.Sender));
                    var recipient = context.Users.FirstOrDefault(u => u.Username.Equals(msg.Recipient));

                    context.UserMessages.Add(new UserMessage()
                    {
                        Content = msg.Content,
                        DateTime = datetime,
                        Sender = sender,
                        Recipient = recipient
                    });

                    Console.WriteLine("Message: " + msg.Content);
                }

                context.SaveChanges();
            }
        }

        private static void TestSeedMethod(ChatSystemContext context)
        {
            var channels = context.Channels
                .Select(c => new
                {
                    c.Name,
                    Messages = c.ChannelMessages.Select(m => new
                    {
                        m.Content,
                        SentOn = m.Datetime,
                        m.User.Username
                    })
                });

            foreach (var channel in channels)
            {
                Console.WriteLine(channel.Name);

                foreach (var msg in channel.Messages)
                {
                    Console.WriteLine("Content: {0} / Sent on: {1} / User {2}",
                        msg.Content,
                        msg.SentOn.ToShortDateString(),
                        msg.Username);
                }

                Console.WriteLine();
            }
        }
    }
}
