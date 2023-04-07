using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;

namespace PortfolioMaster.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;

        public PortfolioService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Other existing methods...

        public async Task<List<Portfolio>> GetPortfoliosByUserId(string userId)
        {
            return await _context.Portfolios.Where(p => p.UserId == userId).ToListAsync();
        }
    }

}
