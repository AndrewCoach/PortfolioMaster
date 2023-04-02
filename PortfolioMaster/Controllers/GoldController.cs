using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PortfolioMaster.Helpers;
using PortfolioMaster.Models;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortfolioMaster.Controllers
{
    public class GoldController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;

        public GoldController(
            IMemoryCache cache, 
            ApplicationDbContext context,
            IConfiguration configuration, 
            IHttpClientFactory httpClientFactory, 
            UserManager<User> userManager)
        {
            _cache = cache;
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }

        // GET: Gold
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var goldHoldings = await _context.Golds
                .Where(h => h.User.Id == userId)
                .Include(h => h.AssetHoldings)
                .ToListAsync();

            var silverHoldings = await _context.Silvers
                .Where(h => h.User.Id == userId)
                .Include(h => h.AssetHoldings)
                .ToListAsync();

            decimal goldPrice = await GetLatestGoldPriceAsync();
            decimal silverPrice = await GetLatestSilverPriceAsync();

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
                _context.Add(gold);
                await _context.SaveChangesAsync();
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

            var gold = await _context.Golds
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gold == null)
            {
                return NotFound();
            }

            return View(gold);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gold = await _context.Golds.FindAsync(id);
            if (gold == null)
            {
                return NotFound();
            }
            return View(gold);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId")] Gold gold)
        {
            if (id != gold.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gold);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoldExists(gold.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gold);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gold = await _context.Golds
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gold == null)
            {
                return NotFound();
            }

            return View(gold);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gold = await _context.Golds.FindAsync(id);
            _context.Golds.Remove(gold);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoldExists(int id)
        {
            return _context.Golds.Any(e => e.Id == id);
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

