using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;
using System.Collections.Generic;

namespace PortfolioMaster.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;

        public PortfolioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Portfolio>> GetPortfoliosByUserId(string userId)
        {
            var portfolios = await _context.Portfolios.Include(p => p.AssetHoldings).ThenInclude(ah => ah.Asset).Where(p => p.UserId == userId).ToListAsync();
            foreach (var portfolio in portfolios)
            {
                portfolio.TotalValue = GetPortfolioValue(portfolio);
            }
            return portfolios;
        }

        public async Task<Portfolio> GetPortfolioById(int id)
        {
            return await _context.Portfolios.FindAsync(id);
        }

        public async Task CreatePortfolio(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePortfolio(Portfolio portfolio)
        {
            _context.Entry(portfolio).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePortfolio(int id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio != null)
            {
                _context.Portfolios.Remove(portfolio);
                await _context.SaveChangesAsync();
            }
        }

        private decimal GetPortfolioValue(Portfolio portfolio)
        {
            decimal totalValue = 0;
            foreach (var assetHolding in portfolio.AssetHoldings)
            {
                if (assetHolding.TransactionType == TransactionType.Purchase)
                {
                    totalValue += assetHolding.Price;
                }
                else if (assetHolding.TransactionType == TransactionType.Sale)
                {
                    totalValue -= assetHolding.Price;
                }
            }
            return totalValue;
        }
        public async Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> GetPortfolioByIdAsync(int id, string userId)
        {
            return await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task UpdatePortfolioAsync(Portfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
            await _context.SaveChangesAsync();
        }
    }
}
