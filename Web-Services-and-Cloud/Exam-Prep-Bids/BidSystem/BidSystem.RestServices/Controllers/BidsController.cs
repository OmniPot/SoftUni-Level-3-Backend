namespace BidSystem.RestServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using BidSystem.Data.Contracts;
    using BidSystem.RestServices.Infrastructure;
    using BidSystem.RestServices.Models.ViewModels;
    using Microsoft.AspNet.Identity;

    [Authorize]
    [RoutePrefix("api/bids")]
    public class BidsController : BaseApiController
    {
        public BidsController()
        {
        }

        public BidsController(IBidsData data, IUserIdProvider provider)
            : base(data, provider)
        {
        }

        [HttpGet]
        [Route("my")]
        public IHttpActionResult UserBids()
        {
            var loggedUserId = this.UserIdProvider.GetUserId();

            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            var bids = this.Data.Bids.All()
                .Where(b => b.Bidder.Id.Equals(loggedUserId))
                .OrderByDescending(b => b.BidDate)
                .Select(BidViewModel.Create);

            return this.Ok(bids);
        }

        [HttpGet]
        [Route("won")]
        public IHttpActionResult UserWonBids()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var wonBids = this.Data.Bids.All()
                .Where(b => b.BidderId.Equals(loggedUserId) &&
                            b.Offer.ExpirationDateTime < DateTime.Now &&
                            b.Price.Equals(b.Offer.Bids.Max(bb => bb.Price)))
                .OrderByDescending(b => b.BidDate)
                .Select(WonBidViewModel.Create);

            return this.Ok(wonBids);
        }
    }
}