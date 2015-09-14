namespace Restaurants.Services.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Restaurants.Models;

    public class TownViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<Town, TownViewModel>> Create
        {
            get
            {
                return town => new TownViewModel()
                {
                    Id = town.Id,
                    Name = town.Name
                };
            }
        }
    }
}