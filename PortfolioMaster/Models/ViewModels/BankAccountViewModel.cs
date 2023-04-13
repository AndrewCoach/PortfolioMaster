using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models.ViewModels
{
    public class BankAccountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal InterestRate { get; set; }

        [Required]
        public decimal TotalValue { get; set; }
    }
}
