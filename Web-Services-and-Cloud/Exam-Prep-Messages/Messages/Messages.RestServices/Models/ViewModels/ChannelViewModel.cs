namespace Messages.RestServices.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Messages.Data.Models;

    public class ChannelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<Channel, ChannelViewModel>> Create
        {
            get
            {
                return channel => new ChannelViewModel
                {
                    Id = channel.Id,
                    Name = channel.Name
                };
            }
        }
    }
}