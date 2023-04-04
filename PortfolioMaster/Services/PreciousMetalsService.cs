using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PortfolioMaster.Controllers;
using PortfolioMaster.Models;
using PortfolioMaster.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

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

        public async Task AddAssetHoldingAsync(AssetHolding assetHolding)
        {
            _context.AssetHoldings.Add(assetHolding);
            await _context.SaveChangesAsync();
        }

        public async Task<Asset> GetAssetById(int assetId)
        {
            return await _context.Assets.FirstOrDefaultAsync(a => a.Id == assetId);
        }

        public async Task<List<Gold>> GetUserGoldHoldingsAsync(string userId)
        {
            return await _context.Golds
                .Where(h => h.UserId == userId)
                .Include(h => h.AssetHoldings)
                .ToListAsync();
        }

        public async Task<List<Silver>> GetUserSilverHoldingsAsync(string userId)
        {
            return await _context.Silvers
                .Where(h => h.UserId == userId)
                .Include(h => h.AssetHoldings)
                .ToListAsync();
        }

        public async Task<AssetHolding> GetAssetHoldingAsync(int id, string userId)
        {
            return await _context.AssetHoldings
                .Where(h => h.Asset.UserId == userId)
                .Include(h => h.Asset)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Gold> GetGoldAsync(int id, string userId)
        {
            return await _context.Golds
                .Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Silver> GetSilverAsync(int id, string userId)
        {
            return await _context.Silvers
                .Where(h => h.UserId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> CreateAssetHoldingAsync(AssetHolding holding, string userId)
        {
            if (holding == null || holding.Asset.UserId != userId)
                return false;

            _context.AssetHoldings.Add(holding);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAssetHoldingAsync(UpdateAssetHoldingDto holdingDto, string userId)
        {
            var holding = await _context.AssetHoldings
                 .Include(h => h.Asset) // Include the related Asset
                 .Include(h => h.Portfolio)
                 .SingleOrDefaultAsync(h => h.Id == holdingDto.Id);

            if (holding == null || holding.Asset.UserId != userId)
                return false;

            // Apply changes to the holding object
            holding.Quantity = holdingDto.Quantity;
            holding.PurchasePrice = holdingDto.PurchasePrice;
            holding.PurchaseDate = holdingDto.PurchaseDate;
            if (holdingDto.PortfolioId.HasValue)
            {
                holding.PortfolioId = holdingDto.PortfolioId.Value;
            }
            if (holdingDto.AssetId.HasValue)
            {
                holding.AssetId = holdingDto.AssetId.Value;
            }
            // ... apply other changes if necessary

            _context.Entry(holding).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAssetHoldingAsync(int id, string userId)
        {
            var holding = await GetAssetHoldingAsync(id, userId);
            if (holding == null)
                return false;

            _context.AssetHoldings.Remove(holding);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateGoldAsync(Gold gold, string userId)
        {
            if (gold == null || gold.UserId != userId)
                return false;

            _context.Golds.Add(gold);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateGoldAsync(Gold gold, string userId)
        {
            if (gold == null || gold.UserId != userId)
                return false;

            _context.Entry(gold).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteGoldAsync(int id, string userId)
        {
            var gold = await GetGoldAsync(id, userId);
            if (gold == null)
                return false;

            _context.Golds.Remove(gold);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateSilverAsync(Silver silver, string userId)
        {
            if (silver == null || silver.UserId != userId)
                return false;

            _context.Silvers.Add(silver);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateSilverAsync(Silver silver, string userId)
        {
            if (silver == null || silver.UserId != userId)
                return false;

            _context.Entry(silver).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSilverAsync(int id, string userId)
        {
            var silver = await GetSilverAsync(id, userId);
            if (silver == null)
                return false;

            _context.Silvers.Remove(silver);
            await _context.SaveChangesAsync();
            return true;
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

