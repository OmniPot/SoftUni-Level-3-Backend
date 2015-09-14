namespace OnlineShop.Services.Controllers
{
    using System.Web.Http;
    using Data;
    using Infrastructure;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new OnlineShopData(
                new OnlineShopContext())
            , new AspNetUserIdProvider())
        {
        }

        public BaseApiController(
            IOnlineShopData data,
            IUserIdProvider userProvider)
        {
            this.Data = data;
            this.UserProvider = userProvider;
        }

        protected IOnlineShopData Data { get; set; }

        protected IUserIdProvider UserProvider { get; set; }
    }
}