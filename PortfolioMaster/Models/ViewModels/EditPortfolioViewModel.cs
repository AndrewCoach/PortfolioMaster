using System.ComponentModel.DataAnnotations;

namespace PortfolioMaster.Models.ViewModels
{
    public class EditPortfolioViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Name { get; set; }
        public ICollection<AssetHolding> AssetHoldings { get; set; } = new List<AssetHolding>();
    }
}
