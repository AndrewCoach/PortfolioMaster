using Microsoft.AspNetCore.Mvc.Rendering;

namespace PortfolioMaster.Models.ViewModels
{
    public class AssetHoldingsViewModel
    {
        public IEnumerable<AssetHolding> AssetHoldings { get; set; }
        public IEnumerable<SelectListItem> Assets { get; set; }
    }

}
