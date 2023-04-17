using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models.ViewModels
{
    public class UpdateAssetHoldingViewModel
    {
        public int Id { get; set; }
        public int? PortfolioId { get; set; }
        public int? AssetId { get; set; }

        [Required(ErrorMessage = "Transaction date is required.")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Display(Name = "Quantity")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Display(Name = "Total Price (USD)")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}
