using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;

public class StockDataUpdater
{
    private readonly ApplicationDbContext _context;
    private readonly string _apiKey;
    private readonly string _marketIndexSymbol;

    public StockDataUpdater(IConfiguration configuration, ApplicationDbContext context)
    {
        _context = context;
        _apiKey = configuration.GetValue<string>("AlphaVantage:ApiKey");
        _marketIndexSymbol = configuration.GetValue<string>("AlphaVantage:MarketIndexSymbol");
    }

    public async Task UpdateStockDataAsync()
    {
        var stocks = await _context.Stocks.ToListAsync();

        using var httpClient = new HttpClient();

        foreach (var stock in stocks)
        {
            if (!string.IsNullOrWhiteSpace(stock.TickerSymbol))
            {
                // Call the API for stock data
                var stockDataUrl = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={stock.TickerSymbol}&apikey={_apiKey}";
                var response = await httpClient.GetAsync(stockDataUrl);
                var content = await response.Content.ReadAsStringAsync();

                // Process the API response
                var json = JObject.Parse(content);
                var timeSeries = json["Time Series (Daily)"] as JObject;

                var stockPriceHistories = new List<StockPriceHistory>();

                foreach (var dailyData in timeSeries)
                {
                    DateTime date = DateTime.Parse(dailyData.Key);
                    decimal price = decimal.Parse(dailyData.Value["5. adjusted close"].ToString());

                    stockPriceHistories.Add(new StockPriceHistory
                    {
                        StockId = stock.Id,
                        Date = date,
                        Price = price
                    });
                }

                // Update the PriceHistories and other attributes
                stock.PriceHistories = stockPriceHistories;

                await UpdateStockAttributesAsync(stock);

                // Calculate Dividend CAGR and Alpha
                await CalculateDividendCAGRAndAlphaAsync(stock);

                _context.Entry(stock).State = EntityState.Modified;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateStockAttributesAsync(Stock stock)
    {
        using var httpClient = new HttpClient();

        // Call the API for stock attributes
        var stockAttributesUrl = $"https://www.alphavantage.co/query?function=OVERVIEW&symbol={stock.TickerSymbol}&apikey={_apiKey}";
        var response = await httpClient.GetAsync(stockAttributesUrl);
        var content = await response.Content.ReadAsStringAsync();

        // Process the API response
        var json = JObject.Parse(content);

        // Update the Stock object with the attributes from the API
        stock.MarketCapitalization = decimal.Parse(json["MarketCapitalization"].ToString());
        stock.EarningsPerShare = decimal.Parse(json["TrailingEps"].ToString());
        stock.PriceToEarningsRatio = decimal.Parse(json["PERatio"].ToString());
        stock.DividendYield = decimal.Parse(json["DividendYield"].ToString());
        stock.PriceToSalesRatio = decimal.Parse(json["PriceToSalesRatio"].ToString());
        stock.PriceToBookRatio = decimal.Parse(json["PriceToBookRatio"].ToString());
        stock.DebtToEquityRatio = decimal.Parse(json["DebtToEquityRatio"].ToString());
        stock.ReturnOnEquity = decimal.Parse(json["ReturnOnEquityTTM"].ToString());
        stock.ReturnOnAssets = decimal.Parse(json["ReturnOnAssetsTTM"].ToString());
        stock.High52WeekPrice = decimal.Parse(json["52WeekHigh"].ToString());
        stock.Low52WeekPrice = decimal.Parse(json["52WeekLow"].ToString());
    }

    public async Task CalculateDividendCAGRAndAlphaAsync(Stock stock)
    {
        using var httpClient = new HttpClient();

        // Call the API for monthly adjusted time series
        var timeSeriesUrl = $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY_ADJUSTED&symbol={stock.TickerSymbol}&apikey={_apiKey}";
        var response = await httpClient.GetAsync(timeSeriesUrl);
        var content = await response.Content.ReadAsStringAsync();

        // Process the API response
        var json = JObject.Parse(content);
        var timeSeries = json["Monthly Adjusted Time Series"] as JObject;

        // Calculate Dividend CAGR
        var dividends = new List<decimal>();
        var startingYear = DateTime.Now.Year - 5; // Choose the starting year for the calculation (e.g., 5 years ago)
        var years = DateTime.Now.Year - startingYear;

        foreach (var monthlyData in timeSeries)
        {
            DateTime date = DateTime.Parse(monthlyData.Key);
            decimal dividend = decimal.Parse(monthlyData.Value["7. dividend amount"].ToString());

            if (date.Year >= startingYear)
            {
                dividends.Add(dividend);
            }
        }

        decimal beginningValue = dividends.First();
        decimal endingValue = dividends.Last();

        stock.DividendCAGR = (decimal)Math.Pow((double)(endingValue / beginningValue), 1.0 / years) - 1;

        // Retrieve Beta
        var betaUrl = $"https://www.alphavantage.co/query?function=BETA&symbol={stock.TickerSymbol}&interval=weekly&time_period=60&series_type=close&apikey={_apiKey}";
        response = await httpClient.GetAsync(betaUrl);
        content = await response.Content.ReadAsStringAsync();

        json = JObject.Parse(content);
        decimal beta = decimal.Parse(json["Realtime Global Beta"]["Beta"].ToString());

        // Retrieve the risk-free rate (10-year Treasury bond yield)
        var riskFreeRateUrl = $"https://www.alphavantage.co/query?function=FRED&symbol=GS10&apikey={_apiKey}";
        response = await httpClient.GetAsync(riskFreeRateUrl);
        content = await response.Content.ReadAsStringAsync();

        json = JObject.Parse(content);
        decimal riskFreeRate = decimal.Parse(json["Realtime Global FRED"].ToString()) / 100;

        // Get benchmark index's historical prices from the API
        string benchmarkSymbol = _marketIndexSymbol; // Use the appropriate symbol for your chosen benchmark index (e.g., ^GSPC for S&P 500)
        var benchmarkTimeSeriesUrl = $"https://www.alphavantage.co/query?function=TIME_SERIES_MONTHLY_ADJUSTED&symbol={benchmarkSymbol}&apikey={_apiKey}";
        var benchmarkResponse = await httpClient.GetAsync(benchmarkTimeSeriesUrl);
        var benchmarkContent = await benchmarkResponse.Content.ReadAsStringAsync();

        // Process the API response
        var benchmarkJson = JObject.Parse(benchmarkContent);
        var benchmarkTimeSeries = benchmarkJson["Monthly Adjusted Time Series"] as JObject;

        var benchmarkPriceHistories = new List<StockPriceHistory>();

        foreach (var monthlyData in benchmarkTimeSeries)
        {
            DateTime date = DateTime.Parse(monthlyData.Key);
            decimal adjustedClose = decimal.Parse(monthlyData.Value["5. adjusted close"].ToString());

            benchmarkPriceHistories.Add(new StockPriceHistory
            {
                Date = date,
                Price = adjustedClose
            });
        }

        // Calculate the stock's return and benchmark index's return
        decimal stockBeginningPrice = stock.PriceHistories.OrderBy(p => p.Date).First().Price;
        decimal stockEndingPrice = stock.PriceHistories.OrderByDescending(p => p.Date).First().Price;
        decimal stockReturn = (stockEndingPrice - stockBeginningPrice) / stockBeginningPrice;

        decimal benchmarkBeginningPrice = benchmarkPriceHistories.OrderBy(p => p.Date).First().Price;
        decimal benchmarkEndingPrice = benchmarkPriceHistories.OrderByDescending(p => p.Date).First().Price;
        decimal benchmarkReturn = (benchmarkEndingPrice - benchmarkBeginningPrice) / benchmarkBeginningPrice;

        decimal expectedReturn = riskFreeRate + beta * (benchmarkReturn - riskFreeRate);
        stock.Alpha = stockReturn - expectedReturn;
    }
}

