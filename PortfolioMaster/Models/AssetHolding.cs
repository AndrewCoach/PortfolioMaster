using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models
{
    public class AssetHolding
    {
        public int Id { get; set; }

        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Quantity (Oz)")]
        public decimal Quantity { get; set; }

        [Display(Name = "Purchase Price (USD)")]
        public decimal PurchasePrice { get; set; }

        // Foreign Key for Portfolio
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        // Foreign Key for Asset
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
    }
}
