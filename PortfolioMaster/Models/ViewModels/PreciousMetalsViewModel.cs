namespace PortfolioMaster.Models.ViewModels
{
    public class PreciousMetalsViewModel
    {
        public IEnumerable<IEnumerable<AssetViewModel>> PreciousMetalsHoldings { get; set; }
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

        public TransactionType TransactionType => AssetHolding.TransactionType;

        public DateTime TransactionDate => AssetHolding.TransactionDate;

        public decimal Quantity => AssetHolding.Quantity;

        public decimal Price => AssetHolding.Price;
        public decimal QuantityTimesCurrentPrice => AssetHolding.Quantity * CurrentPrice;
        public decimal ProfitLoss => TransactionType == TransactionType.Purchase 
            ? AssetHolding.Quantity * CurrentPrice - AssetHolding.Price 
            : AssetHolding.Quantity * AssetHolding.Price - CurrentPrice;
    }
}
