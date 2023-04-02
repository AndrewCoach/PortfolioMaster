using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            // Check if any data exists, if so, return
            if (context.Users.Any() || context.Portfolios.Any() || context.Assets.Any() || context.AssetHoldings.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();

            // Create users
            var user1 = new User
            {
                UserName = "devuser",
                Email = "devuser@example.com",
            };

            var result = await userManager.CreateAsync(user1, "DevUser@123");
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Unable to create user in Initialize method.");
            }

            await signInManager.SignInAsync(user1, isPersistent: false);

            // Create portfolios
            var portfolio1 = new Portfolio { Name = "User1 Portfolio", User = user1 };
            context.Portfolios.Add(portfolio1);

            // Create assets
            var gold1 = new Gold { Name = "Gold1", User = user1 };
            context.Golds.Add(gold1);

            var silver1 = new Silver { Name = "Silver1", User = user1 };
            context.Silvers.Add(silver1);

            await context.SaveChangesAsync();

            // Create asset holdings
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

