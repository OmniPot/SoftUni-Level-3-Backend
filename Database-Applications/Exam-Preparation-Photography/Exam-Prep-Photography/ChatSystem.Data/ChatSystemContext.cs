namespace ChatSystem.Data
{
    using System.Data.Entity;
    using Migrations;
    using Phonebook.Models;

    public class ChatSystemContext : DbContext
    {
        public ChatSystemContext()
            : base("ChatSystemContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChatSystemContext, Configuration>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<ChatSystemContext>());
        }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Channel> Channels { get; set; }

        public virtual IDbSet<UserMessage> UserMessages { get; set; }

        public virtual IDbSet<ChannelMessage> ChannelMessages { get; set; }
    }
}