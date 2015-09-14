namespace Messages.RestServices.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Messages.Data;
    using Messages.Data.Models;
    using Messages.RestServices.Models.BindingModels;
    using Messages.RestServices.Models.ViewModels;

    [RoutePrefix("api/channels")]
    public class ChannelsController : BaseApiController
    {
        public ChannelsController()
        {
        }

        public ChannelsController(IMessagesData data)
            : base(data)
        {
        }

        // GET /api/channels
        [HttpGet]
        public IHttpActionResult ListChannels()
        {
            var channels = this.Data.Channels
                .All()
                .OrderBy(ch => ch.Name)
                .Select(ChannelViewModel.Create);

            return this.Ok(channels);
        }

        // GET /api/channels/{channelId}
        [HttpGet]
        [Route("{channelId}")]
        public IHttpActionResult GetChannelById(int channelId)
        {
            var channel = this.Data.Channels
                .All()
                .Where(c => c.Id == channelId)
                .Select(ChannelViewModel.Create)
                .FirstOrDefault();

            if (channel == null)
            {
                return this.NotFound();
            }

            return this.Ok(channel);
        }

        // POST /api/channels
        [HttpPost]
        public IHttpActionResult CreateChannel(ChannelBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingChannel = this.Data.Channels.All()
                .Where(c => c.Name.Equals(model.Name))
                .Select(ChannelViewModel.Create)
                .FirstOrDefault();

            if (existingChannel != null)
            {
                return this.Conflict();
            }

            var newChannel = new Channel { Name = model.Name };

            this.Data.Channels.Add(newChannel);
            this.Data.SaveChanges();

            var channelViewModel = new ChannelViewModel
            {
                Id = newChannel.Id,
                Name = newChannel.Name
            };

            return this.CreatedAtRoute(
                "DefaultApi",
                new
                {
                    controller = "channels",
                    newChannel.Id
                },
                channelViewModel);
        }

        // PUT /api/channels/{channelId}
        [HttpPut]
        [Route("{channelId}")]
        public IHttpActionResult EditChannel(int channelId, ChannelBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingChannel = this.Data.Channels.Find(channelId);
            var existingChannelName = this.Data.Channels.All()
                .Any(c => c.Name.Equals(model.Name));

            if (existingChannel == null)
            {
                return this.NotFound();
            }

            if (existingChannelName)
            {
                return this.Conflict();
            }

            existingChannel.Name = model.Name;

            this.Data.Channels.Update(existingChannel);
            this.Data.SaveChanges();

            var responseMessage = string.Format("Channel {0} edited successfully.", existingChannel.Id);

            return this.Ok(responseMessage);
        }

        // DELETE /api/channels/{channelId}
        [HttpDelete]
        [Route("{channelId}")]
        public IHttpActionResult DeleteChannel(int channelId)
        {
            var channelToDelete = this.Data.Channels.All()
                .FirstOrDefault(c => c.Id.Equals(channelId));

            if (channelToDelete == null)
            {
                return this.NotFound();
            }

            if (channelToDelete.Messages.Any())
            {
                return this.Conflict();
            }

            this.Data.Channels.Delete(channelToDelete);
            this.Data.SaveChanges();

            var responseMessage = string.Format("Channel {0} deleted.", channelToDelete.Id);

            return this.Ok(responseMessage);
        }
    }
}