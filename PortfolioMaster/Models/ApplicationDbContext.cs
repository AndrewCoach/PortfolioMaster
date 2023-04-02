using Microsoft.EntityFrameworkCore;

namespace PortfolioMaster.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Gold> Golds { get; set; }
        public DbSet<Silver> Silvers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<PeerToPeerLoan> PeerToPeerLoans { get; set; }
    }
}


