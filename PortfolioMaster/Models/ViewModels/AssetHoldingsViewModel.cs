using Microsoft.AspNetCore.Mvc.Rendering;

namespace PortfolioMaster.Models.ViewModels
{
    public class AssetHoldingsViewModel
    {
        public IEnumerable<AssetHolding> GroupedAssetHoldings { get; set; }
        public IEnumerable<AssetHolding> AssetHoldings { get; set; }
        public IEnumerable<SelectListItem> Assets { get; set; }
    }

}
