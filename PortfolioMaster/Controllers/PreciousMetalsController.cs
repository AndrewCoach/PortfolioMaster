using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;
using PortfolioMaster.Models.ViewModels;
using PortfolioMaster.Services;

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

            var preciousMetalsHoldings = await _preciousMetalsService.GetUserPreciousMetalsHoldingsAsync(userId);
            decimal goldPrice = await _preciousMetalsService.GetMetalPriceAsync(MetalType.Gold);
            decimal silverPrice = await _preciousMetalsService.GetMetalPriceAsync(MetalType.Silver);
            ViewBag.GoldPrice = goldPrice;
            ViewBag.SilverPrice = silverPrice;

            var preciousMetalsHoldingsVM = preciousMetalsHoldings.Select(metalHoldings => metalHoldings.Select(m => {
                var metalPrice = m.MetalType == MetalType.Gold ? goldPrice : silverPrice;

                return new AssetViewModel
                {
                    Asset = m,
                    AssetHoldings = m.AssetHoldings.Select(ah => new AssetHoldingViewModel
                    {
                        AssetHolding = ah,
                        CurrentPrice = metalPrice
                    }).ToList(),
                    CurrentPrice = metalPrice
                };
            }));

            return View(new PreciousMetalsViewModel { PreciousMetalsHoldings = preciousMetalsHoldingsVM });
        }

    }
}

