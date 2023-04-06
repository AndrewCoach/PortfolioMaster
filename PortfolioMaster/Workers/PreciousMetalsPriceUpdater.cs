using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PortfolioMaster.Models;
using PortfolioMaster.Services;

namespace PortfolioMaster.Workers
{
    public class PreciousMetalsPriceUpdater
    {
        private readonly PreciousMetalsService _preciousMetalsService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;

        public PreciousMetalsPriceUpdater(PreciousMetalsService preciousMetalsService, IConfiguration configuration, IMemoryCache cache, IHttpClientFactory httpClientFactory)
        {
            _preciousMetalsService = preciousMetalsService;
            _configuration = configuration;
            _cache = cache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task UpdatePrices()
        {
            // Fetch gold and silver prices from the API
            var goldPrice = await GetGoldPriceAsync();
            var silverPrice = await GetSilverPriceAsync();

            // Update the gold and silver prices in the database
            await _preciousMetalsService.CreateOrUpdateGoldPriceAsync(goldPrice);
            await _preciousMetalsService.CreateOrUpdateSilverPriceAsync(silverPrice);
        }

        private async Task<decimal> GetGoldPriceAsync()
        {
            var goldPrice = await _preciousMetalsService.GetMetalPriceAsync(MetalType.Gold);

            if (goldPrice == 0)
            {
                goldPrice = await GetGoldPriceFromAPI();

                // Update the gold price in the database
                await _preciousMetalsService.CreateOrUpdateGoldPriceAsync(goldPrice);
            }

            return goldPrice;
        }

        private async Task<decimal> GetSilverPriceAsync()
        {
            var silverPrice = await _preciousMetalsService.GetMetalPriceAsync(MetalType.Silver);

            if (silverPrice == 0)
            {
                silverPrice = await GetSilverPriceFromAPI();

                // Update the silver price in the database
                await _preciousMetalsService.CreateOrUpdateSilverPriceAsync(silverPrice);
            }

            return silverPrice;
        }

        private async Task<decimal> GetGoldPriceFromAPI()
        {
            if (!_cache.TryGetValue<decimal>("GoldPrice", out decimal goldPrice))
            {
                var apiKey = _configuration["GoldApi:ApiKey"];
                var url = $"https://metals-api.com/api/latest?access_key={apiKey}&base=USD&symbols=XAU";

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    goldPrice = result.rates.XAU;

                    // Convert the gold price to USD per ounce
                    goldPrice = 1 / goldPrice;

                    // Cache the gold price with a 1-day absolute expiration
                    _cache.Set("GoldPrice", goldPrice, TimeSpan.FromDays(1));
                }
            }

            return goldPrice;
        }

        private async Task<decimal> GetSilverPriceFromAPI()
        {
            if (!_cache.TryGetValue<decimal>("SilverPrice", out decimal silverPrice))
            {
                var apiKey = _configuration["GoldApi:ApiKey"];
                var url = $"https://metals-api.com/api/latest?access_key={apiKey}&base=USD&symbols=XAG";

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    silverPrice = result.rates.XAG;

                    // Convert the gold price to USD per ounce
                    silverPrice = 1 / silverPrice;

                    // Cache the silver price with a 1-day absolute expiration
                    _cache.Set("SilverPrice", silverPrice, TimeSpan.FromDays(1));
                }
            }

            return silverPrice;
        }
    }
}