namespace Restaurants.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class RatingBindingModel
    {
        [Required]
        [Range(0, 10)]
        public int Stars { get; set; }
    }
}