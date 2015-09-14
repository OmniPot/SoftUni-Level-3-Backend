namespace Battleships.WebServices.Models
{
    using System.ComponentModel.DataAnnotations;

    public class JoinGameBindingModel
    {
        [Required]
        [MinLength(36)]
        [MaxLength(36)]
        public string GameId { get; set; }
    }
}