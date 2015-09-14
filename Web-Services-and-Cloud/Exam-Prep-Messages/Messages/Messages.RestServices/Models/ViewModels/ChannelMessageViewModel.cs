namespace Messages.RestServices.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Messages.Data.Models;

    public class ChannelMessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }

        public string ChannelName { get; set; }

        public static Expression<Func<ChannelMessage, ChannelMessageViewModel>> Create
        {
            get
            {
                return channelMessage => new ChannelMessageViewModel
                {
                    Id = channelMessage.Id,
                    Text = channelMessage.Text,
                    DateSent = channelMessage.DateSent,
                    Sender = channelMessage.Sender != null ? channelMessage.Sender.UserName : null ,
                    ChannelName = channelMessage.Channel.Name
                };
            }
        }
    }
}