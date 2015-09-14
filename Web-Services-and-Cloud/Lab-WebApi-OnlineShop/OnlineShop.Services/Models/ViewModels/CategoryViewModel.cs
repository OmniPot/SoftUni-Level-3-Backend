namespace OnlineShop.Services.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using OnlineShop.Models;

    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<Category, CategoryViewModel>> GetViewModel
        {
            get
            {
                return category => new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name
                };
            }
        }
    }
}