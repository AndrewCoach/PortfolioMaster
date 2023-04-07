using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;
using PortfolioMaster.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioMaster.Services
{
    public class AssetHoldingService
    {
        private readonly ApplicationDbContext _context;

        public AssetHoldingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssetHolding>> GetAllHoldingsForUserAsync(string userId)
        {
            return await _context.AssetHoldings
                .Include(ah => ah.Asset)
                .Include(ah => ah.Portfolio)
                .Where(ah => ah.Asset.UserId == userId)
                .ToListAsync();
        }

        public async Task<AssetHolding> GetHoldingByIdAsync(int id, string userId)
        {
            return await _context.AssetHoldings
                .Include(ah => ah.Asset)
                .Include(ah => ah.Portfolio)
                .Where(ah => ah.Id == id && ah.Asset.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAssetHoldingAsync(AssetHolding assetHolding)
        {
            _context.AssetHoldings.Add(assetHolding);
            await _context.SaveChangesAsync();
        }

        public async Task<CreateAssetHoldingViewModel> CreateHoldingAsync(CreateAssetHoldingViewModel assetHolding)
        {
            var holding = new AssetHolding
            {
                PurchaseDate = assetHolding.PurchaseDate,
                AssetId = assetHolding.AssetId,
                PortfolioId = assetHolding.PortfolioId,
                PurchasePrice = assetHolding.PurchasePrice,
                Quantity = assetHolding.Quantity
            };

            _context.AssetHoldings.Add(holding);
            await _context.SaveChangesAsync();
            return assetHolding;
        }

        public async Task<bool> UpdateAssetHoldingAsync(UpdateAssetHoldingViewModel holdingDto, string userId)
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

            _context.Entry(holding).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteHoldingAsync(int id, string userId)
        {
            var assetHolding = await GetHoldingByIdAsync(id, userId);

            if (assetHolding == null)
            {
                return false;
            }

            _context.AssetHoldings.Remove(assetHolding);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

