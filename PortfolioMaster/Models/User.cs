namespace PortfolioMaster.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Navigation property for Portfolios
        public ICollection<Portfolio> Portfolios { get; set; }
    }

}
