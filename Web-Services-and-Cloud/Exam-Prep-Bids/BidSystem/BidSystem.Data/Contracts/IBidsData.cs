namespace BidSystem.Data.Contracts
{
    using BidSystem.Data.Models;

    public interface IBidsData
    {
        IRepository<Offer> Offers { get; }

        IRepository<Bid> Bids { get; }

        IRepository<User> Users { get; }

        int SaveChanges();
    }
}