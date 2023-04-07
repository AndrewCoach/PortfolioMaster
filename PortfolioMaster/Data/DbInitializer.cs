using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Contexts;
using PortfolioMaster.Models;
using System;
using System.Linq;

namespace PortfolioMaster.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context.Users.Any() || context.Portfolios.Any() || context.Assets.Any() || context.AssetHoldings.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var user1 = new User
            {
                UserName = "devuser@example.com",
                Email = "devuser@example.com",
            };

            var result = await userManager.CreateAsync(user1, "DevUser@123");
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Unable to create user in Initialize method.");
            }

            await context.SaveChangesAsync();

            var createdUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user1.Email);

            var portfolio1 = new Portfolio { Name = "User1 Portfolio", UserId = createdUser.Id };
            context.Portfolios.Add(portfolio1);

            var gold1 = new PreciousMetal { Name = "Gold", UserId = createdUser.Id };
            context.PreciousMetals.Add(gold1);

            var silver1 = new PreciousMetal { Name = "Silver", UserId = createdUser.Id };
            context.PreciousMetals.Add(silver1);

            await context.SaveChangesAsync();

            var assetHolding1 = new AssetHolding
            {
                PurchaseDate = DateTime.Now.AddMonths(-3),
                Quantity = 10,
                PurchasePrice = 1200,
                PortfolioId = portfolio1.Id,
                AssetId = gold1.Id
            };
            context.AssetHoldings.Add(assetHolding1);

            var assetHolding2 = new AssetHolding
            {
                PurchaseDate = DateTime.Now.AddMonths(-1),
                Quantity = 20,
                PurchasePrice = 22,
                PortfolioId = portfolio1.Id,
                AssetId = silver1.Id
            };
            context.AssetHoldings.Add(assetHolding2);

            await context.SaveChangesAsync();
        }

    }
}

