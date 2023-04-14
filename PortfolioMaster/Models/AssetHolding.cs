using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models
{
    public class AssetHolding
    {
        public int Id { get; set; }

        public TransactionType TransactionType { get; set; }

        public DateTime TransactionDate { get; set; }

        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Display(Name = "Price (USD)")]
        public decimal Price { get; set; }

        // Foreign Key for Portfolio
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        // Foreign Key for Asset
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
    }

    public enum TransactionType
    {
        Purchase,
        Sale,
    }
}
