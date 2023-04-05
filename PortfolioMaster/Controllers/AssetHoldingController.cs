using Microsoft.AspNetCore.Mvc;
using PortfolioMaster.Services;
using PortfolioMaster.Models;
using PortfolioMaster.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

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

        public AssetHoldingController(AssetHoldingService assetHoldingService,
            IMemoryCache cache,
            ApplicationDbContext context,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            UserManager<User> userManager,
            PreciousMetalsService preciousMetalsService,
            IPortfolioService portfolioService)
        {
            _assetHoldingService = assetHoldingService;
            _cache = cache;
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _preciousMetalsService = preciousMetalsService;
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var assetHoldings = await _assetHoldingService.GetAllHoldingsForUserAsync(userId);
            return View(assetHoldings);
        }

        [HttpGet, ActionName("Create")]
        public async Task<IActionResult> CreateAssetHolding(int assetId)
        {
            var userId = _userManager.GetUserId(User);
            var portfolios = await _portfolioService.GetPortfoliosByUserId(userId);
            var asset = await _preciousMetalsService.GetAssetById(assetId);

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
                var assetHolding = new AssetHolding
                {
                    AssetId = model.AssetId,
                    PortfolioId = model.PortfolioId,
                    Quantity = model.Quantity,
                    PurchaseDate = model.PurchaseDate,
                    PurchasePrice = model.PurchasePrice
                };

                await _preciousMetalsService.AddAssetHoldingAsync(assetHolding);
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
            var assetHolding = await _preciousMetalsService.GetAssetHoldingAsync(id.Value, userId);

            if (assetHolding == null)
            {
                return NotFound();
            }

            var dto = new UpdateAssetHoldingDto
            {
                PurchaseDate = assetHolding.PurchaseDate,
                AssetId = assetHolding.AssetId,
                Id = id.Value,
                PortfolioId = assetHolding.PortfolioId,
                PurchasePrice = assetHolding.PurchasePrice,
                Quantity = assetHolding.Quantity
            };

            return View(dto);
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
                bool updated = await _preciousMetalsService.UpdateAssetHoldingAsync(holding, userId);

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
            var assetHolding = await _preciousMetalsService.GetAssetHoldingAsync(id.Value, userId);

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
            bool deleted = await _preciousMetalsService.DeleteAssetHoldingAsync(id, userId);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}

