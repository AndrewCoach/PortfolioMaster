using System.Collections.Generic;


namespace PortfolioMaster.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalValue { get; set; }

        // Foreign Key for User
        public int UserId { get; set; }
        public User User { get; set; }

        // Navigation property for Asset Holdings
        public ICollection<AssetHolding> AssetHoldings { get; set; }
    }

}
