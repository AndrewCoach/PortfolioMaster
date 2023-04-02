using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioMaster.Models
{
    public abstract class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        // Add this User navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Navigation property for Asset Holdings
        public ICollection<AssetHolding> AssetHoldings { get; set; }
    }
}
