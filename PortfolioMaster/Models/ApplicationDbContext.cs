using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PortfolioMaster.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSet properties for User and Portfolio
        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        public DbSet<Gold> Golds { get; set; }
        public DbSet<Silver> Silvers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<PeerToPeerLoan> PeerToPeerLoans { get; set; }
        public DbSet<AssetHolding> AssetHoldings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GoldConfiguration());
            modelBuilder.ApplyConfiguration(new SilverConfiguration());
            modelBuilder.ApplyConfiguration(new StockConfiguration());
            modelBuilder.ApplyConfiguration(new PeerToPeerLoanConfiguration());
            modelBuilder.ApplyConfiguration(new PortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new AssetHoldingConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}



