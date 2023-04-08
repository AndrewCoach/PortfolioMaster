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
    public class PortfolioController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(UserManager<User> userManager, IPortfolioService portfolioService)
        {
            _userManager = userManager;
            _portfolioService = portfolioService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var portfolios = await _portfolioService.GetPortfoliosByUserId(userId);

            var viewModel = new PortfolioIndexViewModel
            {
                Portfolios = portfolios,
                SelectedPortfolioId = portfolios.FirstOrDefault()?.Id ?? 0
            };

            return View(viewModel);
        }

        [HttpGet("{portfolioId}")]
        public async Task<IActionResult> GetPortfolioData(int portfolioId)
        {
            var userId = _userManager.GetUserId(User);
            var portfolio = await _portfolioService.GetPortfolioByIdAsync(portfolioId, userId);

            if (portfolio == null)
            {
                return NotFound();
            }

            var groupedData = portfolio.AssetHoldings
                .GroupBy(ah => ah.TransactionDate.Date)
                .OrderBy(g => g.Key);

            var data = groupedData.Aggregate(new List<(DateTime date, decimal value)>(), (result, current) =>
            {
                decimal previousValue = result.LastOrDefault().value; // Get the value of the last element, or 0 if the list is empty
                decimal currentValue = current.Sum(ah => ah.TransactionType == TransactionType.Purchase ? ah.Price : -ah.Price);
                decimal cumulativeValue = previousValue + currentValue;
                result.Add((current.Key, cumulativeValue));
                return result;
            });

            return Json(data.Select(x => new { date = x.date, value = x.value }));
        }


        // GET: Portfolio/Create
        public IActionResult Create()
        {
            return View();
        }

        // Create action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePortfolioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var portfolio = new Portfolio
                {
                    Name = model.Name,
                    UserId = userId
                };
                await _portfolioService.CreatePortfolioAsync(portfolio);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Edit actions
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var portfolio = await _portfolioService.GetPortfolioByIdAsync(id.Value, userId);
            if (portfolio == null)
            {
                return NotFound();
            }

            var viewModel = new EditPortfolioViewModel
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                AssetHoldings = portfolio.AssetHoldings,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPortfolioViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                var portfolio = await _portfolioService.GetPortfolioByIdAsync(model.Id, userId);
                if (portfolio == null)
                {
                    return NotFound();
                }

                portfolio.Name = model.Name;
                // Reload asset holdings
                portfolio.AssetHoldings = await _portfolioService.GetAssetHoldingsByPortfolioId(model.Id);

                await _portfolioService.UpdatePortfolioAsync(portfolio);
                return RedirectToAction(nameof(Index));
            }
            // Reload asset holdings before returning the model to the view
            model.AssetHoldings = await _portfolioService.GetAssetHoldingsByPortfolioId(model.Id);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var portfolio = await _portfolioService.GetPortfolioById(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            ViewData["AssetHoldingsCount"] = portfolio.AssetHoldings.Count;
            return View(portfolio);
        }


        // POST: Portfolio/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _portfolioService.DeletePortfolio(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
