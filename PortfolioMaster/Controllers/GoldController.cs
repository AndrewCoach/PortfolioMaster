using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortfolioMaster.Helpers;
using PortfolioMaster.Models;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortfolioMaster.Controllers
{
    [Authorize]
    public class GoldController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;

        public GoldController(ApplicationDbContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory, UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }


        // GET: Gold
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (GlobalData.GoldPriceLastFetched < DateTime.UtcNow.AddHours(-24))
            {
                //var goldPrice = await GetLatestGoldPriceAsync();
                var goldPrice = (decimal)0.0002;
                if (goldPrice > 0)
                {
                    GlobalData.GoldPrice = goldPrice;
                    GlobalData.GoldPriceLastFetched = DateTime.UtcNow;
                }
            }

            var userGoldHoldings = _context.Golds
                .Include(g => g.AssetHoldings)
                .Where(g => g.UserId == userId);

            ViewBag.GoldPrice = GlobalData.GoldPrice;

            return View(await userGoldHoldings.ToListAsync());
        }

        private async Task<decimal> GetLatestGoldPriceAsync()
        { 
            var apiKey = _configuration["GoldApi:ApiKey"];
            var url = $"https://metals-api.com/api/latest?access_key={apiKey}&base=USD&symbols=XAU";

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                decimal goldPrice = result.rates.XAU;
                return goldPrice;
            }
            else
            {
                throw new Exception("Error fetching gold price from API");
            }
        }

    }
}

