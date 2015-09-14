namespace Messages.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public string RecipientId { get; set; }

        [ForeignKey("RecipientId")]
        public virtual User Recipient { get; set; }
    }
}