namespace BidSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Bid
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        public DateTime BidDate { get; set; }

        public string Comment { get; set; }

        [Required]
        public string BidderId { get; set; }

        [ForeignKey("BidderId")]
        public virtual User Bidder { get; set; }

        [Required]
        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }
    }
}
