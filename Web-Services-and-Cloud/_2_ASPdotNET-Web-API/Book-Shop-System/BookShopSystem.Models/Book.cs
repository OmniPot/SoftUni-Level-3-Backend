namespace BookShopSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enumerations;

    public class Book
    {
        private ICollection<Category> categories;
        private ICollection<Purchase> purchases;

        public Book()
        {
            this.categories = new HashSet<Category>();
            this.purchases = new HashSet<Purchase>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Copies { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public Edition EditionType { get; set; }

        public virtual Author Author { get; set; }

        public virtual ICollection<Category> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }

        public virtual ICollection<Purchase> Purchases
        {
            get { return this.purchases; }
            set { this.purchases = value; }
        }
    }
}
