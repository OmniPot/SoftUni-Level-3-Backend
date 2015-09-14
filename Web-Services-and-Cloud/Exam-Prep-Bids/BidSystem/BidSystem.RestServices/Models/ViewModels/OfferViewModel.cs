namespace BidSystem.RestServices.Models.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BidSystem.Data.Models;

    public class OfferViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublishData { get; set; }

        public decimal InitialPrice { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public bool IsExpired { get; set; }

        public int BidsCount { get; set; }

        public string BidWinnder { get; set; }

        public IQueryable<BidViewModel> Bids { get; set; }

        public static Expression<Func<Offer, OfferViewModel>> Create
        {
            get
            {
                return offer => new OfferViewModel
                {
                    Id = offer.Id,
                    Title = offer.Title,
                    Description = offer.Description,
                    PublishData = offer.PublishData,
                    InitialPrice = offer.InitialPrice,
                    ExpirationDateTime = offer.ExpirationDateTime,
                    IsExpired = offer.ExpirationDateTime < DateTime.Now,
                    BidsCount = offer.BidsCount,
                    BidWinnder = offer.ExpirationDateTime < DateTime.Now && offer.BidsCount > 0 ?
                        offer.Bids.OrderByDescending(b => b.Price).FirstOrDefault().Bidder.UserName : null,
                    Bids = offer.Bids
                        .AsQueryable()
                        .OrderBy(b => b.BidDate)
                        .Select(BidViewModel.Create)
                };
            }
        }
    }
}