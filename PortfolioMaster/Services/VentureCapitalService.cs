using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Contexts;
using PortfolioMaster.Data;
using PortfolioMaster.Models;
using PortfolioMaster.Models.ViewModels;

namespace PortfolioMaster.Services
{
    public class VentureCapitalService : IVentureCapitalService
    {
        private readonly ApplicationDbContext _context;

        public VentureCapitalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VentureCapital>> GetAllAsync(string userId)
        {
            return await _context.VentureCapitalAssets.Where(vc => vc.UserId == userId).ToListAsync();
        }

        public async Task<VentureCapital> GetByIdAsync(int id, string userId)
        {
            return await _context.VentureCapitalAssets.SingleOrDefaultAsync(vc => vc.Id == id && vc.UserId == userId);
        }

        public async Task CreateAsync(VentureCapital ventureCapital)
        {
            _context.VentureCapitalAssets.Add(ventureCapital);
            await _context.SaveChangesAsync();
        }

        public async Task<VentureCapital> GetVentureCapitalWithHoldingsAsync(int id, string userId)
        {
            return await _context.VentureCapitalAssets
                .Include(vc => vc.AssetHoldings)
                .ThenInclude(vc => vc.Portfolio)
                .SingleOrDefaultAsync(vc => vc.Id == id && vc.UserId == userId);
        }

        public async Task UpdateVentureCapitalAsync(VentureCapitalViewModel ventureCapitalViewModel)
        {
            var ventureCapital = await _context.VentureCapitalAssets.FindAsync(ventureCapitalViewModel.Id);

            if (ventureCapital != null)
            {
                ventureCapital.Name = ventureCapitalViewModel.Name;
                ventureCapital.TotalValue = ventureCapitalViewModel.TotalValue;

                _context.VentureCapitalAssets.Update(ventureCapital);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var ventureCapital = await GetByIdAsync(id, userId);
            if (ventureCapital != null)
            {
                _context.VentureCapitalAssets.Remove(ventureCapital);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteVentureCapitalWithHoldingsAsync(int id, string userId)
        {
            var ventureCapital = await GetByIdAsync(id, userId);
            if (ventureCapital != null)
            {
                var assetHoldings = _context.AssetHoldings.Where(ah => ah.AssetId == id);
                _context.AssetHoldings.RemoveRange(assetHoldings);
                _context.VentureCapitalAssets.Remove(ventureCapital);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTotalValueAsync(int ventureCapitalId, decimal newTotalValue)
        {
            var ventureCapital = await _context.VentureCapitalAssets.FindAsync(ventureCapitalId);
            if (ventureCapital != null)
            {
                ventureCapital.TotalValue = newTotalValue;
                await _context.SaveChangesAsync();
            }
        }
    }
}
