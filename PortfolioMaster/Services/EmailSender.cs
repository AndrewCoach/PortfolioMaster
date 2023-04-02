using Microsoft.AspNetCore.Identity.UI.Services;

namespace PortfolioMaster.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implement your email sending logic here
            // For now, let's just return a completed task without doing anything
            return Task.CompletedTask;
        }
    }
}
