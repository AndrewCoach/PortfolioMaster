using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;

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

