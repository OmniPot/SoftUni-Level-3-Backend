namespace Battleships.WebServices.Controllers
{
    using System.Web.Http;
    using Data;

    public abstract class BaseApiController : ApiController
    {
        protected BaseApiController(IBattleshipsData data)
        {
            this.Data = data;
        }

        protected IBattleshipsData Data { get; private set; }
    }
}