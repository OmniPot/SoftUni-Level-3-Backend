namespace Messages.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Messages.Data.Models;
    using Messages.Data.Repositories;
    using News.Data.Repositories;

    public class MessagesData : IMessagesData
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;

        public MessagesData()
            : this(new MessagesDbContext())
        {
        }

        public MessagesData(MessagesDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<UserMessage> UserMessages
        {
            get { return this.GetRepository<UserMessage>(); }
        }

        public IRepository<ChannelMessage> ChannelMessages
        {
            get { return this.GetRepository<ChannelMessage>(); }
        }

        public IRepository<Channel> Channels
        {
            get { return this.GetRepository<Channel>(); }
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof (T);
            if (!this.repositories.ContainsKey(type))
            {
                var typeOfRepository = typeof (GenericRepository<T>);
                var repository = Activator.CreateInstance(typeOfRepository, this.context);

                this.repositories.Add(type, repository);
            }

            return (IRepository<T>) this.repositories[type];
        }
    }
}