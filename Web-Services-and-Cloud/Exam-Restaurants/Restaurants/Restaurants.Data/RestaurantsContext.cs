namespace Restaurants.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Models;

    public class RestaurantsContext : IdentityDbContext<ApplicationUser>, IRestaurantsDbContext
    {
        public RestaurantsContext()
            : base("Restaurants")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<RestaurantsContext, Configuration>());
        }

        public static RestaurantsContext Create()
        {
            return new RestaurantsContext();
        }

        public virtual IDbSet<Rating> Ratings { get; set; }

        public virtual IDbSet<Town> Towns { get; set; }

        public virtual IDbSet<Restaurant> Restaurants { get; set; }

        public virtual IDbSet<Meal> Meals { get; set; }

        public virtual IDbSet<MealType> MealTypes { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

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