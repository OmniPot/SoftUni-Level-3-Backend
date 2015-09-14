namespace Battleships.WebServices.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PlayTurnBindingModel
    {
        [Required(ErrorMessage = "GameId is required!")]
        [MinLength(36)]
        [MaxLength(36)]
        public string GameId { get; set; }

        [Required(ErrorMessage = "Position X is required to play a turn.")]
        [Range(0, 7, ErrorMessage = "The X coordinate must be in the range [1...8].")]
        public int PositionX { get; set; }

        [Required(ErrorMessage = "Position Y is required to play a turn.")]
        [Range(0, 7, ErrorMessage = "The Y coordinate must be in the range [1...8].")]
        public int PositionY { get; set; }
    }
}