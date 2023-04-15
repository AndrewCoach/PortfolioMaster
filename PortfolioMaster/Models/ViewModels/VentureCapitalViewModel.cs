using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PortfolioMaster.Models.ViewModels
{
    public class VentureCapitalViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        public decimal TotalValue { get; set; }

        public List<AssetHolding> AssetHoldings { get; set; }
    }
}
