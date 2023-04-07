using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models.ViewModels
{
    public class UpdateAssetHoldingViewModel
    {
        public int Id { get; set; }
        public int? PortfolioId { get; set; }
        public int? AssetId { get; set; }

        [Required(ErrorMessage = "Purchase date is required.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Display(Name = "Quantity (Oz)")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Purchase price is required.")]
        [Display(Name = "Purchase Price (USD)")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0.")]
        public decimal PurchasePrice { get; set; }
    }
}
