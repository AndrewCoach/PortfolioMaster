using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using PortfolioMaster.Helpers;
using PortfolioMaster.Models;
using PortfolioMaster.Models.Dtos;
using PortfolioMaster.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioMaster.Controllers
{
    public class StocksController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly StockService _stockService;
        private readonly IPortfolioService _portfolioService;
        private readonly AssetHoldingService _holdingService;
        private readonly AssetService _assetService;

        public StocksController(
            IMemoryCache cache,
            ApplicationDbContext context,
            UserManager<User> userManager,
            StockService stockService,
            IPortfolioService portfolioService,
            AssetHoldingService holdingService,
            AssetService assetService)
        {
            _cache = cache;
            _context = context;
            _userManager = userManager;
            _stockService = stockService;
            _portfolioService = portfolioService;
            _holdingService = holdingService;
            _assetService = assetService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var stocksWithHoldings = await _stockService.GetUserStocksWithHoldingsAsync(userId);

            return View(stocksWithHoldings);
        }

        [HttpGet, ActionName("Create")]
        public async Task<IActionResult> CreateAssetHolding(int assetId)
        {
            var userId = _userManager.GetUserId(User);
            var portfolios = await _portfolioService.GetPortfoliosByUserId(userId);
            var asset = _assetService.GetAsset(assetId);

            var viewModel = new CreateAssetHoldingViewModel
            {
                AssetId = assetId,
                AssetName = asset.Name,
            };
            ViewBag.PortfolioList = new SelectList(portfolios, "Id", "Name");

            return View(viewModel);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateAssetHolding(CreateAssetHoldingViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                await _holdingService.CreateHoldingAsync(model);
                return RedirectToAction(nameof(Index));
            }

            // If the ModelState is not valid, repopulate the portfolios dropdown and return to the view.
            var portfolios = await _portfolioService.GetPortfoliosByUserId(userId);
            ViewBag.PortfolioList = new SelectList(portfolios, "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var assetHolding = await _holdingService.GetHoldingByIdAsync(id.Value, userId);

            if (assetHolding == null)
            {
                return NotFound();
            }

            return View(assetHolding);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var assetHolding = await _holdingService.GetHoldingByIdAsync(id.Value, userId);

            if (assetHolding == null)
            {
                return NotFound();
            }

            return View(assetHolding);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PurchaseDate,Quantity,PurchasePrice")] UpdateAssetHoldingDto holding)
        {
            if (id != holding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                bool updated = await _holdingService.UpdateHoldingAsync(holding, userId);

                if (!updated)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the asset holding. Please try again.");
                    return View(holding);
                }

                return RedirectToAction(nameof(Index));
            }

            // If we reach this point, an error occurred, and we should redisplay the form with the validation errors.
            return View(holding);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var assetHolding = await _holdingService.GetHoldingByIdAsync(id.Value, userId);

            if (assetHolding == null)
            {
                return NotFound();
            }

            return View(assetHolding);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            bool deleted = await _holdingService.DeleteHoldingAsync(id, userId);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}




