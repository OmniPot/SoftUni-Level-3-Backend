namespace Messages.RestServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ChannelBindingModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}