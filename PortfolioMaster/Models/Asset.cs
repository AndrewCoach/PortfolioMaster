using System.Collections.Generic;

namespace PortfolioMaster.Models
{
    public abstract class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property for Asset Holdings
        public ICollection<AssetHolding> AssetHoldings { get; set; }
    }
}
