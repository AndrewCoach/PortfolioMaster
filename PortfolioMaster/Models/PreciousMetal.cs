using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioMaster.Models
{
    public class PreciousMetal : Asset
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public MetalType MetalType { get; set; }
    }
}
