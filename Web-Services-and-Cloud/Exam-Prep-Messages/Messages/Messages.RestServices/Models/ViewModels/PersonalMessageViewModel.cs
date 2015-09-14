namespace Messages.RestServices.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using Messages.Data.Models;

    public class PersonalMessageViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }

        public string Recipient { get; set; }

        public static Expression<Func<UserMessage, PersonalMessageViewModel>> Create
        {
            get
            {
                return personalMessage => new PersonalMessageViewModel
                {
                    Id = personalMessage.Id,
                    Text = personalMessage.Text,
                    DateSent = personalMessage.DateSent,
                    Sender = personalMessage.Sender != null ? personalMessage.Sender.UserName : null,
                    Recipient = personalMessage.Recipient.UserName
                };
            }
        }
    }
}