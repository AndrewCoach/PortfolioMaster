namespace PortfolioMaster.Models
{
    public class StockPriceHistory
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }

}
