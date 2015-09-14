namespace Messages.RestServices.Controllers
{
    using System.Web.Http;
    using Messages.Data;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new MessagesData())
        {
        }

        public BaseApiController(IMessagesData data)
        {
            this.Data = data;
        }

        protected IMessagesData Data { get; private set; }
    }
}