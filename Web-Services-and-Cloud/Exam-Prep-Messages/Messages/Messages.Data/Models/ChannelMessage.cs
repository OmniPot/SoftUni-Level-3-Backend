namespace Messages.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ChannelMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Text { get; set; }

        [Required]
        public DateTime DateSent { get; set; }

        public string SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public int ChannelId { get; set; }

        [Required]
        [ForeignKey("ChannelId")]
        public virtual Channel Channel { get; set; }
    }
}