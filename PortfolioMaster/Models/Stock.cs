namespace PortfolioMaster.Models
{
    public class Stock : Asset
    {
        public string TickerSymbol { get; set; }
        public string Exchange { get; set; }

        // Add a new property for the current market price
        public decimal CurrentMarketPrice { get; set; }
    }
}
