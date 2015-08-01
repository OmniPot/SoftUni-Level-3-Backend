namespace Phonebook.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public string DateTime { get; set; }

        public User Recipient { get; set; }

        public User Sender { get; set; }
    }
}
