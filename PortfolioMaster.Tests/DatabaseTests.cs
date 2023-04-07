using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;
using PortfolioMaster.Contexts;

namespace PortfolioMaster.Tests
{
    public class DatabaseTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json");
            var config = configBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        [Fact]
        public async void DeleteAllContentsFromDatabase()
        {
            using var context = GetDbContext();

            // Delete all data from the tables
            context.AssetHoldings.RemoveRange(context.AssetHoldings);
            context.Assets.RemoveRange(context.Assets);
            context.Portfolios.RemoveRange(context.Portfolios);
            context.Users.RemoveRange(context.Users);

            // Save changes
            await context.SaveChangesAsync();

            // Assert that the tables are empty
            Assert.Empty(await context.AssetHoldings.ToListAsync());
            Assert.Empty(await context.Assets.ToListAsync());
            Assert.Empty(await context.Portfolios.ToListAsync());
            Assert.Empty(await context.Users.ToListAsync());
        }
    }
}
