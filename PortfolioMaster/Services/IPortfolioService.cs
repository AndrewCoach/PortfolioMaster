using System.Threading.Tasks;
using PortfolioMaster.Models;

namespace PortfolioMaster.Services
{
    public interface IPortfolioService
    {
        Task<List<Portfolio>> GetPortfoliosByUserId(string userId);
        Task<Portfolio> GetPortfolioById(int id);
        Task CreatePortfolio(Portfolio portfolio);
        Task UpdatePortfolio(Portfolio portfolio);
        Task DeletePortfolio(int id);
        Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio);
        Task<Portfolio> GetPortfolioByIdAsync(int id, string userId);
        Task UpdatePortfolioAsync(Portfolio portfolio);
        Task<List<AssetHolding>> GetAssetHoldingsByPortfolioId(int portfolioId);
    }
}

