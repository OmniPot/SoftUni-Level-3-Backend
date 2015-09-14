namespace BookShopSystem.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class AuthorBindingModel
    {
        private const string LastNameRequiredMessage = "Author last name is required!";
        private const string LastNameLengthMessage = "Author last name length should be between 2 and 20 symbols long.";

        public string FirstName { get; set; }

        [Required(ErrorMessage = LastNameRequiredMessage)]
        [MinLength(2, ErrorMessage = LastNameLengthMessage)]
        [MaxLength(20, ErrorMessage = LastNameLengthMessage)]
        public string LastName { get; set; }
    }
}