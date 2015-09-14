﻿namespace Restaurants.Services.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Restaurants.Models;

    public class MealViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Type { get; set; }

        public static Expression<Func<Meal, MealViewModel>> Create
        {
            get
            {
                return meal => new MealViewModel
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Price = meal.Price,
                    Type = meal.Type.Name
                };
            }
        }

        public static MealViewModel CreateSingle(Meal meal)
        {
            return new MealViewModel
            {
                Id = meal.Id,
                Name = meal.Name,
                Price = meal.Price,
                Type = meal.Type.Name
            };
        }
    }
}