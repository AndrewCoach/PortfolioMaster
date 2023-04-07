using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioMaster.Models.ViewModels
{
    public class CreateAssetHoldingViewModel
    {
        public CreateAssetHoldingViewModel()
        {
            PurchaseDate = DateTime.Now.Date;
        }

        public int AssetId { get; set; }

        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }

        [Required]
        [Display(Name = "Portfolio")]
        public int PortfolioId { get; set; }

        [Required]
        [Range(0.000001, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        [Display(Name = "Quantity (Oz)")]
        public decimal Quantity { get; set; }

        [Required]
        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Purchase price must be greater than zero.")]
        [Display(Name = "Purchase Price (USD)")]
        public decimal PurchasePrice { get; set; }
    }
}
