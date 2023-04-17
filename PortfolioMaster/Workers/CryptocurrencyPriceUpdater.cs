using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PortfolioMaster.Models;
using PortfolioMaster.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PortfolioMaster.Workers
{
    public class CryptocurrencyPriceUpdater
    {
        private readonly CryptoAssetsService _cryptoCurrencyService;
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;

        public CryptocurrencyPriceUpdater(CryptoAssetsService cryptocurrencyService, IMemoryCache cache, IHttpClientFactory httpClientFactory)
        {
            _cryptoCurrencyService = cryptocurrencyService;
            _cache = cache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task UpdatePrices()
        {
            // Fetch Bitcoin and Ethereum prices from the API
            var bitcoinPrice = await GetBitcoinPriceAsync();
            var ethereumPrice = await GetEthereumPriceAsync();

            // Update the Bitcoin and Ethereum prices in the database
            await _cryptoCurrencyService.CreateOrUpdateBitcoinPriceAsync(bitcoinPrice);
            await _cryptoCurrencyService.CreateOrUpdateEthereumPriceAsync(ethereumPrice);
        }

        private async Task<decimal> GetBitcoinPriceAsync()
        {
            if (!_cache.TryGetValue<decimal>("BitcoinPrice", out decimal bitcoinPrice))
            {
                bitcoinPrice = await GetBitcoinPriceFromAPI();

                // Cache the Bitcoin price with a 1-day absolute expiration
                _cache.Set("BitcoinPrice", bitcoinPrice, TimeSpan.FromDays(1));
            }

            return bitcoinPrice;
        }

        private async Task<decimal> GetEthereumPriceAsync()
        {
            if (!_cache.TryGetValue<decimal>("EthereumPrice", out decimal ethereumPrice))
            {
                ethereumPrice = await GetEthereumPriceFromAPI();

                // Cache the Ethereum price with a 1-day absolute expiration
                _cache.Set("EthereumPrice", ethereumPrice, TimeSpan.FromDays(1));
            }

            return ethereumPrice;
        }

        private async Task<decimal> GetBitcoinPriceFromAPI()
        {
            var url = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd";

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                return result.bitcoin.usd;
            }

            return 0;
        }

        private async Task<decimal> GetEthereumPriceFromAPI()
        {
            var url = "https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=usd";

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                return result.ethereum.usd;
            }

            return 0;
        }
    }
}
