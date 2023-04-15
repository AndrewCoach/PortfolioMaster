using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

namespace PortfolioMaster.Models.Configuration
{
    public class VentureCapitalConfiguration : IEntityTypeConfiguration<VentureCapital>
    {
        public void Configure(EntityTypeBuilder<VentureCapital> builder)
        {
            builder.Property(p => p.TotalValue).HasColumnType("decimal(18, 10)");
        }
    }
}
