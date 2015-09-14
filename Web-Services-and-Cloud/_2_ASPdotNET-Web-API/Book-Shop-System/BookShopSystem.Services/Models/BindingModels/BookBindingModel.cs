namespace BookShopSystem.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using BookShopSystem.Models.Enumerations;

    public class BookBindingModel
    {
        private const string TitleRequiredMessage = "Book title is required.";
        private const string TitleLengthMessage = "Book title must be between 2 and 50 characters long.";

        private const string DescriptionLengthMessage = "Book Description must be less than 1000 characters long.";

        private const string PriceRequiredMessage = "Book price is required.";
        private const string CopiesCountRequiredMessage = "Book copies count is required.";
        private const string EditionTypeRequiredMessage = "Book edition type is required.";
        private const string AuthorIdRequiredMessage = "Book author id type is required.";

        [Required(ErrorMessage = TitleRequiredMessage)]
        [MinLength(2, ErrorMessage = TitleLengthMessage)]
        [MaxLength(50, ErrorMessage = TitleLengthMessage)]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = DescriptionLengthMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = PriceRequiredMessage)]
        public double Price { get; set; }

        [Required(ErrorMessage = CopiesCountRequiredMessage)]
        public int Copies { get; set; }

        [Required(ErrorMessage = EditionTypeRequiredMessage)]
        public Edition EditionType { get; set; }

        [Required(ErrorMessage = AuthorIdRequiredMessage)]
        public int AuthorId { get; set; }

        public string Categories { get; set; }
    }
}