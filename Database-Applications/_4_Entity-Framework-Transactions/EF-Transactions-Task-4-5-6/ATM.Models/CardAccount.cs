namespace ATM.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.SqlTypes;

    public class CardAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "char")]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(4)]
        [Column(TypeName = "char")]
        public string CardPIN { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal CardCash { get; set; }
    }
}
