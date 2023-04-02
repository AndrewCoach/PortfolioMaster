using Microsoft.EntityFrameworkCore;

namespace PortfolioMaster.Models
{
    public class ApplicationDbContext : DbContext
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
    }
}



