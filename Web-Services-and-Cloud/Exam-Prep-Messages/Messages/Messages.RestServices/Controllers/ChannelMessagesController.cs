namespace Messages.RestServices.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Messages.Data.Models;
    using Messages.RestServices.Models.BindingModels;
    using Messages.RestServices.Models.ViewModels;
    using Microsoft.AspNet.Identity;

    [RoutePrefix("api/channel-messages")]
    public class ChannelMessagesController : BaseApiController
    {
        // GET api/channel-messages/{channelName}
        [HttpGet]
        [Route("{channelName}")]
        public IHttpActionResult GetChannelMessages(string channelName)
        {
            var existingChannel = this.Data.Channels
                .All()
                .FirstOrDefault(c => c.Name.Equals(channelName));
            if (existingChannel == null)
            {
                return this.NotFound();
            }

            var channelMessages = existingChannel.Messages
                .AsQueryable()
                .OrderByDescending(chm => chm.DateSent)
                .Select(ChannelMessageViewModel.Create);

            return this.Ok(channelMessages);
        }

        // GET api/channel-messages/{channelName}?limit=[1...1000]
        [HttpGet]
        [Route("{channelName}")]
        public IHttpActionResult GetChannelMessagesWithLimit(string channelName, [FromUri] int limit)
        {
            var existingChannel = this.Data.Channels
                .All()
                .FirstOrDefault(c => c.Name.Equals(channelName));
            if (existingChannel == null)
            {
                return this.NotFound();
            }

            var channelMessages = existingChannel.Messages
                .AsQueryable()
                .OrderByDescending(chm => chm.DateSent)
                .Take(limit)
                .Select(ChannelMessageViewModel.Create);

            return this.Ok(channelMessages);
        }

        // POST api/channel-messages/{channelName}/anonymous
        [HttpPost]
        [AllowAnonymous]
        [Route("{channelName}/anonymous")]
        public IHttpActionResult SendAnonymousMessage(string channelName, ChannelMessageBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingChannel = this.Data.Channels.All()
                .FirstOrDefault(c => c.Name.Equals(channelName));
            if (existingChannel == null)
            {
                return this.NotFound();
            }

            var newMessage = new ChannelMessage
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                Channel = existingChannel,
                ChannelId = existingChannel.Id
            };

            this.Data.ChannelMessages.Add(newMessage);
            existingChannel.Messages.Add(newMessage);

            this.Data.Channels.Update(existingChannel);
            this.Data.SaveChanges();

            var responseMessage = string.Format("Anonymous message sent to channel {0}.", existingChannel.Name);

            return this.Ok(new
            {
                newMessage.Id,
                Message = responseMessage
            });
        }

        // POST api/channel-messages/{channelName}/authorized
        [HttpPost]
        [Route("{channelName}/authorized")]
        public IHttpActionResult SendUserMessageToChannel(string channelName, ChannelMessageBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users.Find(loggedUserId);
            var existingChannel = this.Data.Channels.All()
                .FirstOrDefault(c => c.Name.Equals(channelName));
            if (existingChannel == null)
            {
                return this.NotFound();
            }

            var newMessage = new ChannelMessage
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                Channel = existingChannel,
                ChannelId = existingChannel.Id,
                Sender = loggedUser,
                SenderId = loggedUser.Id
            };

            this.Data.ChannelMessages.Add(newMessage);
            existingChannel.Messages.Add(newMessage);

            this.Data.Channels.Update(existingChannel);
            this.Data.SaveChanges();

            var responseMessage = string.Format("Message sent to channel {0}.", existingChannel.Name);

            return this.Ok(new
            {
                newMessage.Id,
                Sender = newMessage.Sender.UserName,
                Message = responseMessage
            });
        }
    }
}