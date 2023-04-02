namespace PortfolioMaster.Models
{
    using System.Collections.Generic;

    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalValue { get; set; }

        // Foreign Key for User
        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation properties for Asset holdings
        public ICollection<Gold> Golds { get; set; }
        public ICollection<Silver> Silvers { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<PeerToPeerLoan> PeerToPeerLoans { get; set; }
    }

}
