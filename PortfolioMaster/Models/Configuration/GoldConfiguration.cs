using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;
namespace PortfolioMaster.Models.Configuration
{
    public class GoldConfiguration : IEntityTypeConfiguration<Gold>
    {
        public void Configure(EntityTypeBuilder<Gold> builder)
        {
        }
    }
}

