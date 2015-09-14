namespace Messages.Data
{
    using Messages.Data.Models;
    using News.Data.Repositories;

    public interface IMessagesData
    {
        IRepository<UserMessage> UserMessages { get; }

        IRepository<ChannelMessage> ChannelMessages { get; }

        IRepository<Channel> Channels { get; }

        IRepository<User> Users { get; }

        int SaveChanges();
    }
}