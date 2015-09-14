namespace BidSystem.RestServices.Controllers
{
    using System.Web.Http;
    using BidSystem.Data;
    using BidSystem.Data.Contracts;
    using BidSystem.RestServices.Infrastructure;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new BidsData(new BidSystemDbContext()), new AspNetUserIdProvider())
        {
        }

        public BaseApiController(IBidsData data, IUserIdProvider provider)
        {
            this.Data = data;
            this.UserIdProvider = provider;
        }

        protected IUserIdProvider UserIdProvider { get; set; }

        protected IBidsData Data { get; private set; }
    }
}