namespace Messages.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Channel
    {
        private ICollection<ChannelMessage> messages;

        public Channel()
        {
            this.messages = new HashSet<ChannelMessage>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<ChannelMessage> Messages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }
    }
}