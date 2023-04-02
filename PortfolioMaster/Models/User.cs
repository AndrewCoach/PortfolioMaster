using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace PortfolioMaster.Models
{
    public class User : IdentityUser
    {
        // You can add any custom properties specific to your application here

        // Navigation property for Portfolios
        public ICollection<Portfolio> Portfolios { get; set; }
    }
}
