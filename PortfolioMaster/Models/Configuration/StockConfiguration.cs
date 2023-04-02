using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.Property(s => s.PurchasePrice).HasColumnType("decimal(18, 4)");
        builder.Property(s => s.Quantity).HasColumnType("decimal(18, 4)");
    }
}
