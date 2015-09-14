namespace BidSystem.Data.Contracts
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using BidSystem.Data.Models;

    public interface IBidsDbContext
    {
        IDbSet<User> Users { get; set; }

        IDbSet<Bid> Bids { get; set; }

        IDbSet<Offer> Offers { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void SaveChanges();
    }
}