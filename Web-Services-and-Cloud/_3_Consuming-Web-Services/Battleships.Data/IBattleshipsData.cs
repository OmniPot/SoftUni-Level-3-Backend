namespace Battleships.Data
{
    using Models;
    using Repositories;

    public interface IBattleshipsData
    {
        IRepository<ApplicationUser> Users { get; }
        GamesRepository Games { get; }
        IRepository<Ship> Ships { get; }
        int SaveChanges();
    }
}