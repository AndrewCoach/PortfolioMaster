﻿using Microsoft.EntityFrameworkCore;
using PortfolioMaster.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortfolioMaster.Models;

namespace PortfolioMaster.Contexts
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
        public DbSet<Asset> Assets { get; set; }
        public DbSet<PreciousMetal> PreciousMetals { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<PeerToPeerLoan> PeerToPeerLoans { get; set; }
        public DbSet<AssetHolding> AssetHoldings { get; set; }
        public DbSet<PreciousMetalPrice> PreciousMetalPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PreciousMetalsConfiguration());
            modelBuilder.ApplyConfiguration(new StockConfiguration());
            modelBuilder.ApplyConfiguration(new PeerToPeerLoanConfiguration());
            modelBuilder.ApplyConfiguration(new PortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new AssetHoldingConfiguration());
            modelBuilder.ApplyConfiguration(new PreciousMetalPriceConfiguration());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PreciousMetalPrice>()
                .Property(p => p.MetalType)
                .HasConversion(new EnumToStringConverter<MetalType>());

            modelBuilder.Entity<PreciousMetal>()
                .Property(p => p.MetalType)
                .HasConversion(new EnumToStringConverter<MetalType>());
        }
    }
}


