using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Models;

namespace PortfolioMaster.Services
{
    public class StockService
    {
        private readonly ApplicationDbContext _context;

        public StockService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stock>> GetUserStocksWithHoldingsAsync(string userId)
        {
            return await _context.Stocks
                .Include(s => s.AssetHoldings)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }


        // Get a single stock by ID
        public async Task<Stock> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        // Get stocks with pagination and filtering
        public async Task<List<Stock>> GetStocksAsync(int pageIndex, int pageSize, string searchTerm)
        {
            var query = _context.Stocks.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s => s.Name.Contains(searchTerm) || s.TickerSymbol.Contains(searchTerm));
            }

            return await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        // Add more CRUD, pagination, and filtering methods as needed

    }

}
