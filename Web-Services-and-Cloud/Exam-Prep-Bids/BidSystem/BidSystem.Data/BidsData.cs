namespace BidSystem.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using BidSystem.Data.Contracts;
    using BidSystem.Data.Models;

    public class BidsData : IBidsData
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;

        public BidsData()
            : this(new BidSystemDbContext())
        {
        }

        public BidsData(BidSystemDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<Bid> Bids
        {
            get { return this.GetRepository<Bid>(); }
        }

        public IRepository<Offer> Offers
        {
            get { return this.GetRepository<Offer>(); }
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
            var type = typeof(T);
            if (!this.repositories.ContainsKey(type))
            {
                var typeOfRepository = typeof(GenericRepository<T>);
                var repository = Activator.CreateInstance(typeOfRepository, this.context);

                this.repositories.Add(type, repository);
            }

            return (IRepository<T>)this.repositories[type];
        }
    }
}