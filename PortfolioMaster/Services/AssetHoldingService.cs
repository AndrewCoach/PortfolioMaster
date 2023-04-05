using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Models;
using PortfolioMaster.Models.Dtos;
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

        public async Task<bool> UpdateHoldingAsync(UpdateAssetHoldingDto updatedHolding, string userId)
        {
            var existingHolding = await GetHoldingByIdAsync(updatedHolding.Id, userId);

            if (existingHolding == null)
            {
                return false;
            }

            existingHolding.PurchaseDate = updatedHolding.PurchaseDate;
            existingHolding.Quantity = updatedHolding.Quantity;
            existingHolding.PurchasePrice = updatedHolding.PurchasePrice;
            if (updatedHolding.PortfolioId.HasValue)
                existingHolding.PortfolioId = updatedHolding.PortfolioId.Value;

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

