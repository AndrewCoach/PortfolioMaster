using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models.ViewModels
{
    public class CreateBankAccountViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Interest Rate (%)")]
        public decimal InterestRate { get; set; }

        [Required]
        [Display(Name = "Total Value (USD)")]
        public decimal TotalValue { get; set; }
    }
}
