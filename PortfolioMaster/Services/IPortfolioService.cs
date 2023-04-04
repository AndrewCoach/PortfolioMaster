using PortfolioMaster.Models;

namespace PortfolioMaster.Services
{
    public interface IPortfolioService
    {
        Task<List<Portfolio>> GetPortfoliosByUserId(string userId);
    }

}
