using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IPortfolioService _portfolioService;

        public PreciousMetalsController(
            IMemoryCache cache,
            ApplicationDbContext context,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            UserManager<User> userManager,
            PreciousMetalsService preciousMetalsService,
            IPortfolioService portfolioService)
        {
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

            var goldHoldings = await _preciousMetalsService.GetUserGoldHoldingsAsync(userId);
            var silverHoldings = await _preciousMetalsService.GetUserSilverHoldingsAsync(userId);

            decimal goldPrice = await _preciousMetalsService.GetMetalPriceAsync(MetalType.Gold);
            decimal silverPrice = await _preciousMetalsService.GetMetalPriceAsync(MetalType.Silver);
            ViewBag.GoldPrice = goldPrice;
            ViewBag.SilverPrice = silverPrice;

            var goldHoldingsVM = goldHoldings.Select(g => new AssetViewModel
            {
                Asset = g,
                AssetHoldings = g.AssetHoldings.Select(ah => new AssetHoldingViewModel
                {
                    AssetHolding = ah,
                    CurrentPrice = goldPrice
                }).ToList(),
                CurrentPrice = goldPrice
            });

            var silverHoldingsVM = silverHoldings.Select(s => new AssetViewModel
            {
                Asset = s,
                AssetHoldings = s.AssetHoldings.Select(ah => new AssetHoldingViewModel
                {
                    AssetHolding = ah,
                    CurrentPrice = silverPrice
                }).ToList(),
                CurrentPrice = silverPrice
            });

            return View(new PreciousMetalsViewModel { GoldHoldings = goldHoldingsVM, SilverHoldings = silverHoldingsVM });
        }
    }
}

