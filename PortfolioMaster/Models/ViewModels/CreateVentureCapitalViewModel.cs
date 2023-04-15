using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models.ViewModels
{
    public class CreateVentureCapitalViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Total Value (USD)")]
        public decimal TotalValue { get; set; }
    }
}

