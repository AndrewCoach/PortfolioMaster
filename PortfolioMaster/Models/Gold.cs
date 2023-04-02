using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioMaster.Models
{
    public class Gold : Asset
    {
        // Add specific properties for Gold if needed
        public string UserId { get; set; }

        // Add this User navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
