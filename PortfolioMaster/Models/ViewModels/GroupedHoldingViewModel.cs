namespace PortfolioMaster.Models.ViewModels
{
    public class GroupedHoldingViewModel : AssetHolding
    {
        // Aggregated properties
        public decimal TotalQuantity { get; set; }
        public decimal AveragePurchasePrice { get; set; }
        public decimal BuySellDiff { get; set; }
        public decimal BuySellPercentage { get; set; }
    }
}
