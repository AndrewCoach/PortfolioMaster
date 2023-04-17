using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioMaster.Models
{
    public class CryptoAsset : Asset
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public CryptoAssetType CryptoAssetType { get; set; }
    }
}
