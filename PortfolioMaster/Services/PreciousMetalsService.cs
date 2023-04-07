using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;

namespace PortfolioMaster.Services
{
    public class PreciousMetalsService
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public PreciousMetalsService(
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

        public async Task<List<List<PreciousMetal>>> GetUserPreciousMetalsHoldingsAsync(string userId)
        {
            var groupedHoldings = new List<List<PreciousMetal>>();

            var metalTypes = Enum.GetValues(typeof(MetalType)).Cast<MetalType>().ToList();

            foreach (var metalType in metalTypes)
            {
                var metalHoldings = await _context.PreciousMetals
                    .Where(h => h.UserId == userId && h.MetalType == metalType)
                    .Include(h => h.AssetHoldings)
                    .ToListAsync();

                groupedHoldings.Add(metalHoldings);
            }

            return groupedHoldings;
        }

        public async Task<List<PreciousMetal>> GetUserGoldHoldingsAsync(string userId)
        {
            return await _context.PreciousMetals
                .Where(h => h.UserId == userId)
                .Where(h => h.UserId == userId && h.MetalType == MetalType.Gold)
                .Include(h => h.AssetHoldings)
                .ToListAsync();
        }

        public async Task<List<PreciousMetal>> GetUserSilverHoldingsAsync(string userId)
        {
            return await _context.PreciousMetals
                .Where(h => h.UserId == userId && h.MetalType == MetalType.Silver)
                .Include(h => h.AssetHoldings)
                .ToListAsync();
        }

        public async Task<PreciousMetal> GetPreciousMetalAsync(int id, string userId)
        {
            return await _context.PreciousMetals
                .Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        #region Metal prices
        public async Task CreateOrUpdateGoldPriceAsync(decimal goldPrice)
        {
            await CreateOrUpdateMetalPriceAsync(MetalType.Gold, goldPrice);
        }

        public async Task CreateOrUpdateSilverPriceAsync(decimal silverPrice)
        {
            await CreateOrUpdateMetalPriceAsync(MetalType.Silver, silverPrice);
        }

        public async Task<decimal> GetMetalPriceAsync(MetalType metalType)
        {
            var metalPrice = await _context.PreciousMetalPrices
                .FirstOrDefaultAsync(m => m.MetalType == metalType);

            return metalPrice?.Price ?? 0;
        }

        private async Task CreateOrUpdateMetalPriceAsync(MetalType metalType, decimal price)
        {
            var metalPrice = await _context.PreciousMetalPrices
                .FirstOrDefaultAsync(m => m.MetalType == metalType);

            if (metalPrice == null)
            {
                metalPrice = new PreciousMetalPrice
                {
                    MetalType = metalType,
                    Price = price
                };
                _context.PreciousMetalPrices.Add(metalPrice);
            }
            else
            {
                metalPrice.Price = price;
            }

            await _context.SaveChangesAsync();
        }
        #endregion
    }
}

