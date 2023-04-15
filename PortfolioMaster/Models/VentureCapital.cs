using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models
{
    public class VentureCapital : Asset
    {
        [Display(Name = "Total Value (USD)")]
        public decimal TotalValue { get; set; }
    }
}
