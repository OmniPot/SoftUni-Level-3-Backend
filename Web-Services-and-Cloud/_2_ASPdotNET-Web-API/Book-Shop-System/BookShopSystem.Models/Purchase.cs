namespace BookShopSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("UserId")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        [Required]
        public double Price { get; set; }

        public DateTime DateOfPurchase { get; set; }

        public bool isRecalled { get; set; }
    }
}
