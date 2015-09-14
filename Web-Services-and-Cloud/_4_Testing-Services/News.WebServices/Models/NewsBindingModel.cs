namespace News.WebServices.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NewsBindingModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string Content { get; set; }

        public DateTime? PublishDate { get; set; }
    }
}