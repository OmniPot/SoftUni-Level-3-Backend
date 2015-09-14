namespace BookShopSystem.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryBindingModel
    {
        private const string RequiredCategoryNameMessage = "Category name is required.";

        [Required(ErrorMessage = RequiredCategoryNameMessage)]
        public string Name { get; set; }
    }
}