namespace BookShopSystem.Services.Models.ViewModels
{
    using System.Linq.Expressions;
    using BookShopSystem.Models;

    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Expression<System.Func<Category, CategoryViewModel>> GetViewModel
        {
            get
            {
                return category => new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name
                };
            }
        }

        public static CategoryViewModel CreateViewModel(Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}