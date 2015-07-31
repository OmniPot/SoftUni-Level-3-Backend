namespace BookShopSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using BookShopSystem.Enumerations;
    using Enumerations;

    public class Book
    {
        public Book()
        {
            this.Categories = new HashSet<Category>();
            this.RelatedBooks = new HashSet<Book>();
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
        public Edition EditionType { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Copies { get; set; }

        [Required]
        public AgeRestriction AgeRestriction { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public virtual Author Author { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Book> RelatedBooks { get; set; } 
    }
}
