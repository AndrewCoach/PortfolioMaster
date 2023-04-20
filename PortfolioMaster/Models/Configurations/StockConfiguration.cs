using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

namespace PortfolioMaster.Models.Configuration
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.Property(p => p.CurrentMarketPrice).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.DividendCAGR).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.Alpha).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.MarketCapitalization).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.EarningsPerShare).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.PriceToEarningsRatio).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.DividendYield).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.PriceToSalesRatio).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.PriceToBookRatio).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.DebtToEquityRatio).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.ReturnOnEquity).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.ReturnOnAssets).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.High52WeekPrice).HasColumnType("decimal(18, 4)");
            builder.Property(p => p.Low52WeekPrice).HasColumnType("decimal(18, 4)");
        }
    }

}
