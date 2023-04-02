namespace PortfolioMaster.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User : IdentityUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Navigation property for Portfolios
        public ICollection<Portfolio> Portfolios { get; set; }
    }

}
