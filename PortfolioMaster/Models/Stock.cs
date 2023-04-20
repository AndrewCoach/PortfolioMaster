namespace PortfolioMaster.Models
{
    public class Stock : Asset
    {
        public string TickerSymbol { get; set; }
        public string Exchange { get; set; }

        public decimal CurrentMarketPrice { get; set; }
        public decimal DividendCAGR { get; set; }
        public decimal Alpha { get; set; }

        public decimal MarketCapitalization { get; set; }
        public decimal EarningsPerShare { get; set; }
        public decimal PriceToEarningsRatio { get; set; }
        public decimal DividendYield { get; set; }
        public decimal PriceToSalesRatio { get; set; }
        public decimal PriceToBookRatio { get; set; }
        public decimal DebtToEquityRatio { get; set; }
        public decimal ReturnOnEquity { get; set; }
        public decimal ReturnOnAssets { get; set; }
        public decimal High52WeekPrice { get; set; }
        public decimal Low52WeekPrice { get; set; }

        // Navigation property for the stock's historical prices
        public ICollection<StockPriceHistory> PriceHistories { get; set; }
    }

}
