namespace Messages.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Messages.Data.Models;

    public interface IMessagesDbContext
    {
        IDbSet<User> Users { get; set; }

        IDbSet<Channel> Channels { get; set; }

        IDbSet<UserMessage> UserMessages { get; set; }

        IDbSet<ChannelMessage> ChannelMessages { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void SaveChanges();
    }
}