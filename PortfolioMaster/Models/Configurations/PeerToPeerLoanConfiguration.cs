﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioMaster.Models;

namespace PortfolioMaster.Models.Configuration
{
    public class PeerToPeerLoanConfiguration : IEntityTypeConfiguration<PeerToPeerLoan>
    {
        public void Configure(EntityTypeBuilder<PeerToPeerLoan> builder)
        {
        }
    }
}

