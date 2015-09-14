namespace BidSystem.Data
{
    using System.Data.Entity;
    using BidSystem.Data.Contracts;
    using BidSystem.Data.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class BidSystemDbContext : IdentityDbContext<User>, IBidsDbContext
    {
        public BidSystemDbContext()
            : base("BidSystem")
        {
        }

        public static BidSystemDbContext Create()
        {
            return new BidSystemDbContext();
        }

        public IDbSet<Offer> Offers { get; set; }

        public IDbSet<Bid> Bids { get; set; }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
