namespace Restaurants.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Restaurants.Models;

    public interface IRestaurantsDbContext
    {
        IDbSet<Rating> Ratings { get; set; }

        IDbSet<Town> Towns { get; set; }

        IDbSet<Restaurant> Restaurants { get; set; }

        IDbSet<Meal> Meals { get; set; }

        IDbSet<MealType> MealTypes { get; set; }

        IDbSet<Order> Orders { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void SaveChanges();
    }
}