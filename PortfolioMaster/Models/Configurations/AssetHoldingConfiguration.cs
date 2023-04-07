using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

namespace PortfolioMaster.Models.Configuration
{
    public class AssetHoldingConfiguration : IEntityTypeConfiguration<AssetHolding>
    {
        public void Configure(EntityTypeBuilder<AssetHolding> builder)
        {
            builder.Property(ah => ah.Quantity).HasColumnType("decimal(18, 4)");
            builder.Property(ah => ah.Price).HasColumnType("decimal(18, 4)");
        }
    }
}
