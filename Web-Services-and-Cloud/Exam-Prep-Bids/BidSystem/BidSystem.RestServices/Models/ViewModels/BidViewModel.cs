namespace BidSystem.RestServices.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using BidSystem.Data.Models;

    public class BidViewModel
    {
        public int Id { get; set; }

        public int OfferId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Bidder { get; set; }

        public decimal OfferPrice { get; set; }

        public string Comment { get; set; }

        public static Expression<Func<Bid, BidViewModel>> Create
        {
            get
            {
                return bid => new BidViewModel
                {
                    Id = bid.Id,
                    OfferId = bid.OfferId,
                    DateCreated = bid.BidDate,
                    Bidder = bid.Bidder.UserName,
                    OfferPrice = bid.Price,
                    Comment = bid.Comment
                };
            }
        }
    }
}