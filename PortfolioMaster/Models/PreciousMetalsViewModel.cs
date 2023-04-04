namespace PortfolioMaster.Models
{
    public class PreciousMetalsViewModel
    {
        public IEnumerable<AssetViewModel> GoldHoldings { get; set; }
        public IEnumerable<AssetViewModel> SilverHoldings { get; set; }
    }

    public class AssetViewModel
    {
        public Asset Asset { get; set; }
        public ICollection<AssetHoldingViewModel> AssetHoldings { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalPurchasePrice => Asset.GetTotalPurchasePrice();
        public decimal TotalValue => Asset.GetTotalValue(CurrentPrice);
        public decimal ProfitLoss => Asset.GetProfitLoss(CurrentPrice);
    }

    public class AssetHoldingViewModel
    {
        public int Id => AssetHolding.Id;
        public AssetHolding AssetHolding { get; set; }
        public decimal CurrentPrice { get; set; }

        public DateTime PurchaseDate => AssetHolding.PurchaseDate;

        public decimal Quantity => AssetHolding.Quantity;

        public decimal PurchasePrice => AssetHolding.PurchasePrice;

        public decimal PurchasePriceTimesQuantity => AssetHolding.PurchasePrice * AssetHolding.Quantity;
        public decimal QuantityTimesCurrentPrice => AssetHolding.Quantity * CurrentPrice;
        public decimal ProfitLoss => (AssetHolding.Quantity * CurrentPrice) - (AssetHolding.PurchasePrice * AssetHolding.Quantity);
    }
}
