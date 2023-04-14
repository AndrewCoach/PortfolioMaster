using System.Collections.Generic;
using System.Threading.Tasks;
using PortfolioMaster.Models;
using PortfolioMaster.Models.ViewModels;

namespace PortfolioMaster.Services
{
    public interface IBankAccountService
    {
        Task<List<BankAccount>> GetAllAsync(string userId);
        Task<BankAccount> GetByIdAsync(int id, string userId);
        Task CreateAsync(BankAccount bankAccount);
        Task UpdateBankAccountAsync(BankAccountViewModel bankAccountViewModel);

        Task<BankAccount> GetBankAccountWithHoldingsAsync(int id, string userId);
        Task DeleteAsync(int id, string userId);
        Task DeleteBankAccountWithHoldingsAsync(int id, string userId);
        Task UpdateTotalValueAsync(int bankAccountId, decimal newTotalValue);
    }
}

