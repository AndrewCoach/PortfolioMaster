using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

namespace PortfolioMaster.Models.Configuration
{
    public class StockPriceHistoryConfiguration : IEntityTypeConfiguration<StockPriceHistory>
    {
        public void Configure(EntityTypeBuilder<StockPriceHistory> builder)
        {
            builder.Property(p => p.Price).HasColumnType("decimal(18, 10)");
        }
    }
}
