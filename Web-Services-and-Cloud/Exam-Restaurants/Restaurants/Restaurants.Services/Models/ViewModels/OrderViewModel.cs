namespace Restaurants.Services.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Restaurants.Models;

    public class OrderViewModel
    {
        public int Id { get; set; }

        public MealViewModel Meal { get; set; }

        public int Quantity { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public static Expression<Func<Order, OrderViewModel>> Create
        {
            get
            {
                return order => new OrderViewModel
                {
                    Id = order.Id,
                    Meal = new MealViewModel
                    {
                        Id = order.MealId,
                        Name = order.Meal.Name,
                        Price = order.Meal.Price,
                        Type = order.Meal.Type.Name
                    },
                    Quantity = order.Quantity,
                    Status = order.OrderStatus,
                    CreatedOn = order.CreatedOn
                };
            }
        }
    }
}