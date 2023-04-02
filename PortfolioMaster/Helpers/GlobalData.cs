using System;

namespace PortfolioMaster.Helpers
{
    public static class GlobalData
    {
        public static decimal GoldPrice { get; set; }
        public static DateTime GoldPriceLastFetched { get; set; } = DateTime.MinValue;
    }
}

