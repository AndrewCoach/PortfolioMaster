namespace PortfolioMaster.Models
{
    public class Stock : Asset
    {
        public string TickerSymbol { get; set; }
        public string Exchange { get; set; }
    }

}
