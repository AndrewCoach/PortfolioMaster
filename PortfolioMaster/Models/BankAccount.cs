using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models
{
    public class BankAccount : Asset
    {
        [Display(Name = "Total Value (USD)")]
        public decimal TotalValue { get; set; }
    }
}
