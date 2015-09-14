namespace Messages.Data
{
    using System.Data.Entity;
    using Messages.Data.Migrations;
    using Messages.Data.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class MessagesDbContext : IdentityDbContext<User>, IMessagesDbContext
    {
        public MessagesDbContext()
            : base("MessagesDbContext")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<MessagesDbContext,
                    MessagesDbMigrationConfiguration>());
        }

        public virtual IDbSet<Channel> Channels { get; set; }

        public virtual IDbSet<ChannelMessage> ChannelMessages { get; set; }

        public virtual IDbSet<UserMessage> UserMessages { get; set; }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public static MessagesDbContext Create()
        {
            return new MessagesDbContext();
        }
    }
}