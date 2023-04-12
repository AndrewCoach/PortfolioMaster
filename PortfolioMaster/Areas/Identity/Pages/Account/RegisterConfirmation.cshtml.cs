using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PortfolioMaster.Areas.Identity.Pages.Account
{
    public class RegisterConfirmationModel : PageModel
    {
        public Task OnGetAsync()
        {
            return Task.CompletedTask;
        }
    }
}

