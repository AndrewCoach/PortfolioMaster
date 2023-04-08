using Microsoft.AspNetCore.Mvc;
using PortfolioMaster.Services;
using PortfolioMaster.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace PortfolioMaster.Controllers
{
    public class AssetHoldingController : Controller
    {
        private readonly AssetHoldingService _assetHoldingService;
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;
        private readonly PreciousMetalsService _preciousMetalsService;
        private readonly IPortfolioService _portfolioService;
        private readonly AssetService _assetService;

        public AssetHoldingController(AssetHoldingService assetHoldingService,
            IMemoryCache cache,
            ApplicationDbContext context,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            UserManager<User> userManager,
            PreciousMetalsService preciousMetalsService,
            IPortfolioService portfolioService,
            AssetService assetService)
        {
            _assetHoldingService = assetHoldingService;
            _cache = cache;
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _preciousMetalsService = preciousMetalsService;
            _portfolioService = portfolioService;
            _assetService = assetService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var assetHoldings = await _assetHoldingService.GetAllHoldingsForUserAsync(userId);
            var assets = _assetService.GetAllAssetsForUser(userId);
            var assetSelectListItems = assets.Select(a => new SelectListItem(a.Name, a.Id.ToString())).ToList();

            var groupedHoldings = assetHoldings
                .GroupBy(ah => new { ah.Asset, ah.Portfolio })
                .Select(g => {

                    var totalBuyValue = g.Where(ah => ah.TransactionType == TransactionType.Purchase).Sum(ah => ah.Price);
                    var totalSellValue = g.Where(ah => ah.TransactionType == TransactionType.Sale).Sum(ah => ah.Price);

                    return new GroupedHoldingViewModel
                    {
                        Asset = g.Key.Asset,
                        Portfolio = g.Key.Portfolio,
                        Quantity = g.Sum(ah => ah.TransactionType == TransactionType.Purchase ? ah.Quantity : -ah.Quantity),
                        TotalQuantity = g.Sum(ah => ah.TransactionType == TransactionType.Purchase ? ah.Quantity : -ah.Quantity),
                        AveragePurchasePrice = g.Where(ah => ah.TransactionType == TransactionType.Purchase).Average(ah => ah.Price),
                        BuySellDiff = totalBuyValue - totalSellValue,
                        BuySellPercentage = totalBuyValue != 0 ? (totalBuyValue - totalSellValue) / totalBuyValue * 100 : 0,
                    };
                })
                .ToList();

            var viewModel = new AssetHoldingsViewModel
            {
                GroupedAssetHoldings = groupedHoldings,
                AssetHoldings = assetHoldings,
                Assets = assetSelectListItems
            };

            return View(viewModel);
        }

        [HttpGet, ActionName("Create")]
        public async Task<IActionResult> CreateAssetHolding(int assetId)
        {
            var userId = _userManager.GetUserId(User);
            var portfolios = await _portfolioService.GetPortfoliosByUserId(userId);
            var asset = _assetService.GetAssetById(assetId);

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
                var transaction = new AssetHolding
                {
                    AssetId = model.AssetId,
                    PortfolioId = model.PortfolioId,
                    Quantity = model.Quantity,
                    TransactionDate = model.TransactionDate,
                    Price = model.Price,
                    TransactionType = model.TransactionType
                };

                await _assetHoldingService.AddAssetHoldingAsync(transaction);
                return RedirectToAction(nameof(Index));
            }

            // If the ModelState is not valid, repopulate the portfolios dropdown and return to the view.
            var portfolios = await _portfolioService.GetPortfoliosByUserId(userId);
            ViewBag.PortfolioList = new SelectList(portfolios, "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var transaction = await _assetHoldingService.GetHoldingByIdAsync(id.Value, userId);

            if (transaction == null)
            {
                return NotFound();
            }

            var dto = new UpdateAssetHoldingViewModel
            {
                TransactionDate = transaction.TransactionDate,
                AssetId = transaction.AssetId,
                Id = id.Value,
                PortfolioId = transaction.PortfolioId,
                Price = transaction.Price,
                Quantity = transaction.Quantity,
                TransactionType = transaction.TransactionType
            };

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TransactionDate,TransactionType,Quantity,Price")] UpdateAssetHoldingViewModel holding)
        {
            if (id != holding.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                bool updated = await _assetHoldingService.UpdateAssetHoldingAsync(holding, userId);

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
            var assetHolding = await _assetHoldingService.GetHoldingByIdAsync(id.Value, userId);

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
            bool deleted = await _assetHoldingService.DeleteHoldingAsync(id, userId);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}

