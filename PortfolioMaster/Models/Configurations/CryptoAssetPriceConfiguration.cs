using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

namespace PortfolioMaster.Models.Configuration
{
    public class CryptoAssetPriceConfiguration : IEntityTypeConfiguration<CryptoAssetPrice>
    {
        public void Configure(EntityTypeBuilder<CryptoAssetPrice> builder)
        {
            builder.Property(p => p.Price).HasColumnType("decimal(18, 10)");
        }
    }
}
