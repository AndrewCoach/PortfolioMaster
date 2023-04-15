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
    public class VentureCapitalController : Controller
    {
        private readonly IVentureCapitalService _ventureCapitalService;
        private readonly UserManager<User> _userManager;
        private readonly AssetHoldingService _assetHoldingService;

        public VentureCapitalController(IVentureCapitalService ventureService, UserManager<User> userManager, AssetHoldingService assetHoldingService)
        {
            _ventureCapitalService = ventureService;
            _userManager = userManager;
            _assetHoldingService = assetHoldingService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var ventures = await _ventureCapitalService.GetAllAsync(userId);
            var allAssetHoldings = await _assetHoldingService.GetAllHoldingsForUserAsync(userId);
            var ventureViewModels = ventures.Select(b => new VentureCapitalViewModel
            {
                Id = b.Id,
                Name = b.Name,
                TotalValue = b.TotalValue,
                AssetHoldings = allAssetHoldings
            .Where(ah => ah.Asset.Id == b.Id)
            .ToList()
            }).ToList();
            return View(ventureViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTotalValue([FromForm] VentureCapitalViewModel vantureViewModel)
        {
            if (ModelState.IsValid)
            {
                await _ventureCapitalService.UpdateVentureCapitalAsync(vantureViewModel);
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User);
            var ventures = await _ventureCapitalService.GetAllAsync(userId);

            var ventureViewModels = ventures.Select(b => new VentureCapitalViewModel
            {
                Id = b.Id,
                Name = b.Name,
                TotalValue = b.TotalValue
            }).ToList();

            return View("Index", ventureViewModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVentureCapitalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ventureCapital = new VentureCapital
                {
                    Name = model.Name,
                    TotalValue = model.TotalValue,
                    UserId = _userManager.GetUserId(User)
                };

                await _ventureCapitalService.CreateAsync(ventureCapital);

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

            var ventureCapital = await _ventureCapitalService.GetVentureCapitalWithHoldingsAsync(id.Value, _userManager.GetUserId(User));
            if (ventureCapital == null)
            {
                return NotFound();
            }

            if (await _assetHoldingService.AssetHasHoldingsAsync(id.Value))
            {
                ViewBag.HasHoldings = true;
            }

            return View(ventureCapital);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ventureCapitalService.DeleteVentureCapitalWithHoldingsAsync(id, _userManager.GetUserId(User));
            return RedirectToAction(nameof(Index));
        }

    }
}
