namespace Restaurants.Data.UnitOfWork
{
    using Restaurants.Data.Repositories;
    using Restaurants.Models;

    public interface IRestaurantsData
    {
        IRepository<ApplicationUser> ApplicationUsers { get; }

        IRepository<Rating> Ratings { get; }

        IRepository<Town> Towns { get; }

        IRepository<Restaurant> Restaurants { get; }

        IRepository<Meal> Meals { get; }

        IRepository<MealType> MealTypes { get; }

        IRepository<Order> Orders { get; }

        int SaveChanges();
    }
}