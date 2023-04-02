using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

namespace PortfolioMaster.Models.Configuration
{
    public class SilverConfiguration : IEntityTypeConfiguration<Silver>
    {
        public void Configure(EntityTypeBuilder<Silver> builder)
        {
        }
    }
}

