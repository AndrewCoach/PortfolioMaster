using System.Collections.Generic;
using System.Threading.Tasks;
using PortfolioMaster.Models;

namespace PortfolioMaster.Services
{
    public interface IBankAccountService
    {
        Task<List<BankAccount>> GetAllAsync(string userId);
        Task<BankAccount> GetByIdAsync(int id, string userId);
        Task CreateAsync(BankAccount bankAccount);
        Task UpdateAsync(BankAccount bankAccount);
        Task DeleteAsync(int id, string userId);
    }
}

