namespace BidSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Offer
    {
        private ICollection<Bid> bids;

        public Offer()
        {
            this.bids = new HashSet<Bid>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublishData { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public decimal InitialPrice { get; set; }

        public string SellerId { get; set; }

        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; }

        public int BidsCount { get; set; }

        public virtual ICollection<Bid> Bids
        {
            get { return this.bids; }
            set { this.bids = value; }
        }
    }
}
