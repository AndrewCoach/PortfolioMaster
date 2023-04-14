using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace PortfolioMaster.Models.ViewModels
{
    public class CreateAssetHoldingViewModel
    {
        public CreateAssetHoldingViewModel()
        {
            TransactionDate = DateTime.Now.Date;
        }

        public int AssetId { get; set; }

        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }

        [Required]
        [Display(Name = "Portfolio")]
        public int PortfolioId { get; set; }

        [Required]
        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }

        [Required]
        [Range(0.000001, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [Display(Name = "Price (USD)")]
        public decimal Price { get; set; }

        public SelectList? PortfolioList { get; set; }
    }
}
