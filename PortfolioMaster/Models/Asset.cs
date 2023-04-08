using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioMaster.Models
{
    public abstract class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        // Add this User navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Navigation property for Asset Holdings
        public ICollection<AssetHolding> AssetHoldings { get; set; }

        public decimal GetTotalPurchasePrice()
        {
            return AssetHoldings.Where(h => h.TransactionType == TransactionType.Purchase).Sum(h => h.Price);
        }

        public decimal GetTotalSalePrice()
        {
            return AssetHoldings.Where(h => h.TransactionType == TransactionType.Sale).Sum(h => h.Price);
        }

        public decimal GetTotalValue(decimal currentPrice)
        {
            decimal totalQuantity = AssetHoldings.Where(h => h.TransactionType == TransactionType.Purchase).Sum(h => h.Quantity) -
                                     AssetHoldings.Where(h => h.TransactionType == TransactionType.Sale).Sum(h => h.Quantity);
            return totalQuantity * currentPrice;
        }

        public decimal GetProfitLoss(decimal currentPrice)
        {
            return GetTotalValue(currentPrice) - (GetTotalPurchasePrice() - GetTotalSalePrice());
        }
    }
}
