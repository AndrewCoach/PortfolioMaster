using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models.Dtos
{ 
    public class UpdateAssetHoldingDto
    {
        public int Id { get; set; }
        public int? PortfolioId { get; set; } // Nullable
        public int? AssetId { get; set; } // Nullable
        // ... other fields
        [Required(ErrorMessage = "Purchase date is required.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Purchase price is required.")]
        [Range(0.00001, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0.")]
        public decimal PurchasePrice { get; set; }
    }
}
