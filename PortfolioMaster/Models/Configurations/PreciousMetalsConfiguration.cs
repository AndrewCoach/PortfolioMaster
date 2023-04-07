using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;
namespace PortfolioMaster.Models.Configuration
{
    public class PreciousMetalsConfiguration : IEntityTypeConfiguration<PreciousMetal>
    {
        public void Configure(EntityTypeBuilder<PreciousMetal> builder)
        {
        }
    }
}

