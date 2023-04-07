using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

namespace PortfolioMaster.Models.Configuration
{
    public class PreciousMetalPriceConfiguration : IEntityTypeConfiguration<PreciousMetalPrice>
    {
        public void Configure(EntityTypeBuilder<PreciousMetalPrice> builder)
        {
            builder.Property(p => p.Price).HasColumnType("decimal(18, 10)");
        }
    }
}
