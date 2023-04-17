using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;
using PortfolioMaster.Models.ViewModels;

namespace PortfolioMaster.Services
{
    public class CryptoAssetsService
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public CryptoAssetsService(
            IMemoryCache cache,
            ApplicationDbContext context,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<List<CryptoAsset>>> GetUserCryptoAssetsHoldingsAsync(string userId)
        {
            var groupedHoldings = new List<List<CryptoAsset>>();

            var cryptoTypes = Enum.GetValues(typeof(CryptoAssetType)).Cast<CryptoAssetType>().ToList();

            foreach (var cryptoType in cryptoTypes)
            {
                var cryptoHoldings = await _context.CryptoAssets
                    .Where(h => h.UserId == userId && h.CryptoAssetType == cryptoType)
                    .Include(h => h.AssetHoldings)
                    .ToListAsync();

                groupedHoldings.Add(cryptoHoldings);
            }

            return groupedHoldings;
        }

        public async Task<Dictionary<CryptoAssetType, List<CryptoPriceHistory>>> GetCryptoPriceHistoriesAsync()
        {
            // Fetch price history data for each crypto asset type and store it in a dictionary
            Dictionary<CryptoAssetType, List<CryptoPriceHistory>> cryptoPriceHistories = new Dictionary<CryptoAssetType, List<CryptoPriceHistory>>();

            foreach (CryptoAssetType assetType in Enum.GetValues(typeof(CryptoAssetType)))
            {
                cryptoPriceHistories[assetType] = await GetCryptoPriceHistoryAsync(assetType);
            }

            return cryptoPriceHistories;
        }

    public async Task<List<CryptoPriceHistory>> GetCryptoPriceHistoryAsync(CryptoAssetType assetType)
        {
            string apiUrl = "https://api.coingecko.com/api/v3/coins/";
            string coinId = assetType == CryptoAssetType.Bitcoin ? "bitcoin" : "ethereum";
            string days = "30"; // Fetch data for the last 30 days
            string vsCurrency = "usd";

            var url = $"{apiUrl}{coinId}/market_chart?vs_currency={vsCurrency}&days={days}&interval=daily";
            var response = await url.GetStringAsync();

            var jsonResponse = JObject.Parse(response);
            var prices = jsonResponse["prices"].ToObject<List<List<decimal>>>();
            var priceHistory = new List<CryptoPriceHistory>();

            foreach (var priceData in prices)
            {
                var date = DateTimeOffset.FromUnixTimeMilliseconds((long)priceData[0]).DateTime;
                var price = priceData[1];

                priceHistory.Add(new CryptoPriceHistory { Date = date, Price = price });
            }

            return priceHistory;
        }

    public async Task<List<CryptoAsset>> GetUserBitcoinHoldingsAsync(string userId)
        {
            return await _context.CryptoAssets
                .Where(h => h.UserId == userId)
                .Where(h => h.UserId == userId && h.CryptoAssetType == CryptoAssetType.Bitcoin)
                .Include(h => h.AssetHoldings)
                .ToListAsync();
        }

        public async Task<List<CryptoAsset>> GetUserEthereumHoldingsAsync(string userId)
        {
            return await _context.CryptoAssets
                .Where(h => h.UserId == userId && h.CryptoAssetType == CryptoAssetType.Ethereum)
                .Include(h => h.AssetHoldings)
                .ToListAsync();
        }

        public async Task<CryptoAsset> GetCryptoAssetAsync(int id, string userId)
        {
            return await _context.CryptoAssets
                .Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        #region Crypto prices
        public async Task CreateOrUpdateBitcoinPriceAsync(decimal bitcoinPrice)
        {
            await CreateOrUpdateCryptoPriceAsync(CryptoAssetType.Bitcoin, bitcoinPrice);
        }

        public async Task CreateOrUpdateEthereumPriceAsync(decimal ethereumPrice)
        {
            await CreateOrUpdateCryptoPriceAsync(CryptoAssetType.Ethereum, ethereumPrice);
        }

        public async Task<decimal> GetCryptoPriceAsync(CryptoAssetType cryptoType)
        {
            var cryptoPrice = await _context.CryptoAssetPrices
                .FirstOrDefaultAsync(m => m.CryptoAssetType == cryptoType);

            return cryptoPrice?.Price ?? 0;
        }

        private async Task CreateOrUpdateCryptoPriceAsync(CryptoAssetType cryptoType, decimal price)
        {
            var cryptoPrice = await _context.CryptoAssetPrices
                .FirstOrDefaultAsync(m => m.CryptoAssetType == cryptoType);

            if (cryptoPrice == null)
            {
                cryptoPrice = new CryptoAssetPrice
                {
                    CryptoAssetType = cryptoType,
                    Price = price,
                    Date= DateTime.UtcNow,
                };
                _context.CryptoAssetPrices.Add(cryptoPrice);
            }
            else
            {
                cryptoPrice.Price = price;
            }

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}

