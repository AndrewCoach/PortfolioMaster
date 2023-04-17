using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;
using PortfolioMaster.Models.ViewModels;
using PortfolioMaster.Services;

namespace PortfolioMaster.Controllers
{
    public class CryptoAssetsController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;
        private readonly CryptoAssetsService _cryptoAssetsService;
        private readonly IPortfolioService _portfolioService;

        public CryptoAssetsController(
            IMemoryCache cache,
            ApplicationDbContext context,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            UserManager<User> userManager,
            CryptoAssetsService cryptoAssetsService,
            IPortfolioService portfolioService)
        {
            _cache = cache;
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _cryptoAssetsService = cryptoAssetsService;
            _portfolioService = portfolioService;
    }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var cryptoHoldings = await _cryptoAssetsService.GetUserCryptoAssetsHoldingsAsync(userId);
            decimal bitcoinPrice = await _cryptoAssetsService.GetCryptoPriceAsync(CryptoAssetType.Bitcoin);
            decimal etherPrice = await _cryptoAssetsService.GetCryptoPriceAsync(CryptoAssetType.Ethereum);
            ViewBag.BitcoinPrice = bitcoinPrice;
            ViewBag.EthereumPrice = etherPrice;

            var cryptoHoldingsVm = cryptoHoldings.Select(cryptoHoldings => cryptoHoldings.Select(m => {
                var metalPrice = m.CryptoAssetType == CryptoAssetType.Bitcoin ? bitcoinPrice : etherPrice;

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

            return View(new CryptoAssetsViewModel { CryptoAssetsHoldings = cryptoHoldingsVm });
        }

    }
}

