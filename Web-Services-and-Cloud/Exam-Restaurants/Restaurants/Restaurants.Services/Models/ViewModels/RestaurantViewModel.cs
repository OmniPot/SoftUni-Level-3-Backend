namespace Restaurants.Services.Models.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Restaurants.Models;

    public class RestaurantViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? Rating { get; set; }

        public TownViewModel Town { get; set; }

        public static Expression<Func<Restaurant, RestaurantViewModel>> Create
        {
            get
            {
                return restaurant => new RestaurantViewModel()
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
                    Rating = restaurant.Ratings.Average(rr => rr.Stars),
                    Town = new TownViewModel
                    {
                        Id = restaurant.TownId,
                        Name = restaurant.Town.Name
                    }
                };
            }
        }

        public static RestaurantViewModel CreateSingle(Restaurant restaurant)
        {
            return new RestaurantViewModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Rating = null,
                Town = new TownViewModel
                {
                    Id = restaurant.TownId,
                    Name = restaurant.Town.Name
                }
            };
        }
    }
}