using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioMaster.Models
{
    public class PreciousMetalPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public MetalType MetalType { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }

    public enum MetalType
    {
        Gold,
        Silver,
    }
}
