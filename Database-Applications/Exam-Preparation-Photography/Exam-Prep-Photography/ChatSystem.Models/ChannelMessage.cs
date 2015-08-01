namespace Phonebook.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChannelMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Datetime { get; set; }

        public virtual User User { get; set; }

        public virtual Channel Channel { get; set; }
    }
}
