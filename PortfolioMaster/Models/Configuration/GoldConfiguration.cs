using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

public class GoldConfiguration : IEntityTypeConfiguration<Gold>
{
    public void Configure(EntityTypeBuilder<Gold> builder)
    {
        builder.Property(g => g.PurchasePrice).HasColumnType("decimal(18, 4)");
        builder.Property(g => g.Quantity).HasColumnType("decimal(18, 4)");
    }
}

