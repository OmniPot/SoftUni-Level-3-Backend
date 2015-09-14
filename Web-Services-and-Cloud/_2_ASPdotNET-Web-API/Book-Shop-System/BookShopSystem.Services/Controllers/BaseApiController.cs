namespace BookShopSystem.Services.Controllers
{
    using System.Web.Http;
    using Data;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new BookShopSystemData())
        {
        }

        public BaseApiController(IBookShopSystemData data)
        {
            this.BookShopData = data;
        }

        protected IBookShopSystemData BookShopData { get; private set; }
    }
}
