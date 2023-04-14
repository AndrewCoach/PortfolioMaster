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
    public class BankAccountService : IBankAccountService
    {
        private readonly ApplicationDbContext _context;

        public BankAccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BankAccount>> GetAllAsync(string userId)
        {
            return await _context.BankAccounts.Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task<BankAccount> GetByIdAsync(int id, string userId)
        {
            return await _context.BankAccounts.SingleOrDefaultAsync(b => b.Id == id && b.UserId == userId);
        }

        public async Task CreateAsync(BankAccount bankAccount)
        {
            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBankAccountAsync(BankAccountViewModel bankAccountViewModel)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(bankAccountViewModel.Id);

            if (bankAccount != null)
            {
                bankAccount.Name = bankAccountViewModel.Name;
                bankAccount.InterestRate = bankAccountViewModel.InterestRate;
                bankAccount.TotalValue = bankAccountViewModel.TotalValue;

                _context.BankAccounts.Update(bankAccount);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var bankAccount = await GetByIdAsync(id, userId);
            if (bankAccount != null)
            {
                _context.BankAccounts.Remove(bankAccount);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBankAccountWithHoldingsAsync(int id, string userId)
        {
            var bankAccount = await GetByIdAsync(id, userId);
            if (bankAccount != null)
            {
                var assetHoldings = _context.AssetHoldings.Where(ah => ah.AssetId == id);
                _context.AssetHoldings.RemoveRange(assetHoldings);
                _context.BankAccounts.Remove(bankAccount);
                await _context.SaveChangesAsync();
            }
        }


        public async Task UpdateTotalValueAsync(int bankAccountId, decimal newTotalValue)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(bankAccountId);
            if (bankAccount != null)
            {
                bankAccount.TotalValue = newTotalValue;
                await _context.SaveChangesAsync();
            }
        }
    }
}

