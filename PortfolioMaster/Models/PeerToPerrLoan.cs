namespace PortfolioMaster.Models
{
    public class PeerToPeerLoan : Asset
    {
        public decimal InterestRate { get; set; }
        public DateTime MaturityDate { get; set; }
    }
}
