using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PortfolioMaster.Helpers;
using PortfolioMaster.Models;
using PortfolioMaster.Models.Dtos;
using PortfolioMaster.Services;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortfolioMaster.Controllers
{
    public class PreciousMetalsController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;
        private readonly PreciousMetalsService _preciousMetalsService;

        public PreciousMetalsController(
            IMemoryCache cache,
            ApplicationDbContext context,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            UserManager<User> userManager,
            PreciousMetalsService preciousMetalsService)
        {
            _cache = cache;
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _preciousMetalsService = preciousMetalsService;
        }

        // GET: Gold
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var goldHoldings = await _preciousMetalsService.GetUserGoldHoldingsAsync(userId);

            var silverHoldings = await _preciousMetalsService.GetUserSilverHoldingsAsync(userId);

            decimal goldPrice = (decimal)0.0005;// await GetLatestGoldPriceAsync();
            decimal silverPrice = (decimal)0.0005;// await GetLatestSilverPriceAsync();

            ViewBag.GoldPrice = goldPrice;
            ViewBag.SilverPrice = silverPrice;

            return View(new PreciousMetalsViewModel { GoldHoldings = goldHoldings, SilverHoldings = silverHoldings });
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserId")] Gold gold)
        {
            if (ModelState.IsValid)
            {
                gold.UserId = _userManager.GetUserId(User);
                await _preciousMetalsService.CreateGoldAsync(gold, gold.UserId);
                return RedirectToAction(nameof(Index));
            }
            return View(gold);
        }

        public async Task<IActionResult> Details(int? id)
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

        private async Task<decimal> GetLatestGoldPriceAsync()
        {
            if (!_cache.TryGetValue<decimal>("GoldPrice", out decimal goldPrice))
            {
                var apiKey = _configuration["GoldApi:ApiKey"];
                var url = $"https://metals-api.com/api/latest?access_key={apiKey}&base=USD&symbols=XAU";

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    goldPrice = result.rates.XAU;

                    // Cache the gold price with a 1-day absolute expiration
                    _cache.Set("GoldPrice", goldPrice, TimeSpan.FromDays(1));
                }
            }

            return goldPrice;
        }

        private async Task<decimal> GetLatestSilverPriceAsync()
        {
            if (!_cache.TryGetValue<decimal>("SilverPrice", out decimal silverPrice))
            {
                var apiKey = _configuration["GoldApi:ApiKey"];
                var url = $"https://metals-api.com/api/latest?access_key={apiKey}&base=USD&symbols=XAG";

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    silverPrice = result.rates.XAG;

                    // Cache the silver price with a 1-day absolute expiration
                    _cache.Set("SilverPrice", silverPrice, TimeSpan.FromDays(1));
                }
            }

            return silverPrice;
        }
    }
}

