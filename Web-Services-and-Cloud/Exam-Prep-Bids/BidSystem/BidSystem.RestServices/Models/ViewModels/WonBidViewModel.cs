namespace BidSystem.RestServices.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using BidSystem.Data.Models;

    public class WonBidViewModel
    {
        public int Id { get; set; }

        public int OfferId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Bidder { get; set; }

        public decimal OfferPrice { get; set; }

        public string Comment { get; set; }

        public static Expression<Func<Bid, WonBidViewModel>> Create
        {
            get
            {
                return bid => new WonBidViewModel()
                {
                    Id = bid.Id,
                    OfferId = bid.OfferId,
                    DateCreated = bid.BidDate,
                    OfferPrice = bid.Offer.InitialPrice,
                    Comment = bid.Comment
                };
            }
        }
    }
}