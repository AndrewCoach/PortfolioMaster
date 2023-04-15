using System.Collections.Generic;
using System.Threading.Tasks;
using PortfolioMaster.Models;
using PortfolioMaster.Models.ViewModels;

namespace PortfolioMaster.Services
{
    public interface IVentureCapitalService
    {
        Task<List<VentureCapital>> GetAllAsync(string userId);
        Task<VentureCapital> GetByIdAsync(int id, string userId);
        Task CreateAsync(VentureCapital ventureCapital);
        Task<VentureCapital> GetVentureCapitalWithHoldingsAsync(int id, string userId);
        Task UpdateVentureCapitalAsync(VentureCapitalViewModel ventureCapitalViewModel);
        Task DeleteAsync(int id, string userId);
        Task DeleteVentureCapitalWithHoldingsAsync(int id, string userId);
        Task UpdateTotalValueAsync(int ventureCapitalId, decimal newTotalValue);
    }
}


