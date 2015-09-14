namespace News.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(2000)]
        public string Content { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }
    }
}