using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Contexts;
using PortfolioMaster.Data;
using PortfolioMaster.Models;

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

        public async Task UpdateAsync(BankAccount bankAccount)
        {
            _context.Entry(bankAccount).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
    }
}

