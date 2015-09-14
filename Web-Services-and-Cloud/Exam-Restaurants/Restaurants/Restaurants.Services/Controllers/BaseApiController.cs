namespace Restaurants.Services.Controllers
{
    using System.Web.Http;
    using Restaurants.Data;
    using Restaurants.Data.UnitOfWork;
    using Restaurants.Services.Infrastructure;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new RestaurantsData(new RestaurantsContext()), new AspNetUserIdProvider())
        {
        }

        public BaseApiController(IRestaurantsData data, IUserIdProvider provider)
        {
            this.Data = data;
            this.UserIdProvider = provider;
        }

        protected IUserIdProvider UserIdProvider { get; set; }

        protected IRestaurantsData Data { get; private set; }
    }
}