namespace PortfolioMaster.Models
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Navigation property for Portfolios
        public ICollection<Portfolio> Portfolios { get; set; }
    }

}
