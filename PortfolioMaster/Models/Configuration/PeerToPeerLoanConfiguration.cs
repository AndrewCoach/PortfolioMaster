using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

public class PeerToPeerLoanConfiguration : IEntityTypeConfiguration<PeerToPeerLoan>
{
    public void Configure(EntityTypeBuilder<PeerToPeerLoan> builder)
    {
        builder.Property(p => p.PurchasePrice).HasColumnType("decimal(18, 4)");
        builder.Property(p => p.Quantity).HasColumnType("decimal(18, 4)");
        builder.Property(p => p.InterestRate).HasColumnType("decimal(18, 4)");
    }
}

