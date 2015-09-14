namespace BidSystem.RestServices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using BidSystem.Data.Models;
    using BidSystem.RestServices.Models.BindingModels;
    using BidSystem.RestServices.Models.ViewModels;
    using Microsoft.AspNet.Identity;

    [Authorize]
    [RoutePrefix("api/offers")]
    public class OffersController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("all")]
        public IHttpActionResult All()
        {
            var offers = this.Data.Offers.All()
                .OrderBy(o => o.PublishData)
                .Select(OfferViewModel.Create);

            return this.Ok(offers);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("active")]
        public IHttpActionResult Active()
        {
            var activeOffers = this.Data.Offers.All()
                .OrderBy(o => o.ExpirationDateTime)
                .Where(o => o.ExpirationDateTime > DateTime.Now)
                .Select(OfferViewModel.Create);

            return this.Ok(activeOffers);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("expired")]
        public IHttpActionResult Expired()
        {
            var expiredOffers = this.Data.Offers.All()
                .OrderBy(o => o.ExpirationDateTime)
                .Where(o => o.ExpirationDateTime < DateTime.Now)
                .Select(OfferViewModel.Create);

            return this.Ok(expiredOffers);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("details/{offerId}")]
        public IHttpActionResult ById(int offerId)
        {
            var offer = this.Data.Offers.All()
                .Where(o => o.Id == offerId)
                .Select(OfferViewModel.Create)
                .FirstOrDefault();

            if (offer == null)
            {
                return this.NotFound();
            }

            return this.Ok(offer);
        }

        [HttpGet]
        [Route("{my}")]
        public IHttpActionResult UserOffers()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            var offers = this.Data.Offers.All()
                .OrderBy(o => o.PublishData)
                .Where(o => o.SellerId == loggedUserId)
                .Select(OfferViewModel.Create);

            return this.Ok(offers);
        }

        [HttpPost]
        public IHttpActionResult CreateOffer(OfferBindingModel offerModel)
        {
            if (offerModel == null)
            {
                return this.BadRequest("Offer model cannot be null.");
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);

            if (loggedUser == null)
            {
                return this.Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newOffer = new Offer
            {
                Title = offerModel.Title,
                Description = offerModel.Description,
                InitialPrice = offerModel.InitialPrice,
                ExpirationDateTime = offerModel.ExpirationDateTime,
                SellerId = loggedUserId,
                Seller = loggedUser,
                PublishData = DateTime.Now,
                BidsCount = 0,
                Bids = new HashSet<Bid>()
            };

            loggedUser.Offers.Add(newOffer);
            this.Data.Users.Update(loggedUser);

            this.Data.Offers.Add(newOffer);
            this.Data.SaveChanges();

            return this.CreatedAtRoute(
                "DefaultApi",
                new
                {
                    Id = newOffer.Id,
                    Seller = loggedUser.UserName,
                    Message = "Successfully created offer."
                },
                new
                {
                    newOffer.Id,
                    newOffer.Title,
                    newOffer.Description,
                    newOffer.InitialPrice,
                    newOffer.ExpirationDateTime
                });
        }

        [HttpPost]
        [Route("{id}/bid")]
        public IHttpActionResult BidForOffer(int id, BidBindingModel bidModel)
        {
            if (bidModel == null)
            {
                return this.BadRequest("Bid data is null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.All()
                .FirstOrDefault(u => u.Id.Equals(loggedUserId));

            var existingOffer = this.Data.Offers.All()
                .FirstOrDefault(o => o.Id.Equals(id));

            if (existingOffer == null)
            {
                return this.NotFound();
            }

            if (existingOffer.ExpirationDateTime < DateTime.Now)
            {
                return this.BadRequest("Cannot bid for expired offer.");
            }

            if (bidModel.BidPrice < existingOffer.InitialPrice)
            {
                return this.BadRequest(string.Format("Your bid should be > {0}", existingOffer.InitialPrice));
            }

            if (existingOffer.Bids.Any())
            {
                var highestBidPrice = existingOffer.Bids.Max(b => b.Price);
                if (bidModel.BidPrice < highestBidPrice)
                {
                    return this.BadRequest(string.Format("Your bid should be > {0}", highestBidPrice));
                }
            }

            var newBid = new Bid
            {
                Price = bidModel.BidPrice,
                BidDate = DateTime.Now,
                Bidder = loggedUser,
                BidderId = loggedUserId,
                Comment = bidModel.Comment,
                Offer = existingOffer,
                OfferId = existingOffer.Id
            };

            this.Data.Bids.Add(newBid);
            existingOffer.Bids.Add(newBid);
            this.Data.Offers.Update(existingOffer);

            this.Data.SaveChanges();

            return this.Ok(new
            {
                Id = newBid.Id,
                Bidder = loggedUser.UserName,
                Message = string.Format("Successfully bidded for offer with Id: {0}.", existingOffer.Id)
            });
        }
    }
}