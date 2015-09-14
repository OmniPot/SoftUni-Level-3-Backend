namespace Messages.RestServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class PersonalMessageBindingModel
    {
        [Required]
        [MaxLength(100)]
        public string Text { get; set; }

        [Required]
        public string Recipient { get; set; }
    }
}