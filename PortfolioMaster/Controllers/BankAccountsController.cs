using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortfolioMaster.Models;
using PortfolioMaster.Models.ViewModels;
using PortfolioMaster.Services;

namespace PortfolioMaster.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class BankAccountsController : Controller
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly UserManager<User> _userManager;
        private readonly AssetHoldingService _assetHoldingService;

        public BankAccountsController(IBankAccountService bankAccountService, UserManager<User> userManager, AssetHoldingService assetHoldingService)
        {
            _bankAccountService = bankAccountService;
            _userManager = userManager;
            _assetHoldingService = assetHoldingService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var bankAccounts = await _bankAccountService.GetAllAsync(userId);
            var allAssetHoldings = await _assetHoldingService.GetAllHoldingsForUserAsync(userId);
            var bankAccountViewModels = bankAccounts.Select(b => new BankAccountViewModel
            {
                Id = b.Id,
                Name = b.Name,
                InterestRate = b.InterestRate,
                TotalValue = b.TotalValue,
                AssetHoldings = allAssetHoldings
            .Where(ah => ah.Asset.Id == b.Id)
            .ToList()
            }).ToList();
            return View(bankAccountViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTotalValue([FromForm] BankAccountViewModel bankAccountViewModel)
        {
            if (ModelState.IsValid)
            {
                await _bankAccountService.UpdateBankAccountAsync(bankAccountViewModel);
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User);
            var bankAccounts = await _bankAccountService.GetAllAsync(userId);

            var bankAccountViewModels = bankAccounts.Select(b => new BankAccountViewModel
            {
                Id = b.Id,
                Name = b.Name,
                InterestRate = b.InterestRate,
                TotalValue = b.TotalValue
            }).ToList();

            return View("Index", bankAccountViewModels);
        }

        // GET: BankAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankAccounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBankAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bankAccount = new BankAccount
                {
                    Name = model.Name,
                    InterestRate = model.InterestRate,
                    TotalValue = model.TotalValue,
                    UserId = _userManager.GetUserId(User)
                };

                await _bankAccountService.CreateAsync(bankAccount);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccount = await _bankAccountService.GetByIdAsync(id.Value, _userManager.GetUserId(User));
            if (bankAccount == null)
            {
                return NotFound();
            }

            if (await _assetHoldingService.BankAccountHasHoldingsAsync(id.Value))
            {
                ViewBag.HasHoldings = true;
            }

            return View(bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bankAccountService.DeleteBankAccountWithHoldingsAsync(id, _userManager.GetUserId(User));
            return RedirectToAction(nameof(Index));
        }

    }
}
