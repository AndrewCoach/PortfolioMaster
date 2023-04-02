using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

public class SilverConfiguration : IEntityTypeConfiguration<Silver>
{
    public void Configure(EntityTypeBuilder<Silver> builder)
    {
        builder.Property(s => s.PurchasePrice).HasColumnType("decimal(18, 4)");
        builder.Property(s => s.Quantity).HasColumnType("decimal(18, 4)");
    }
}

