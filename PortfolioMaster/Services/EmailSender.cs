using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PortfolioMaster.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridApiKey;

        public EmailSender(IOptions<SendGridOptions> optionsAccessor)
        {
            _sendGridApiKey = optionsAccessor.Value.ApiKey;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("kociandreas1@gmail.com", "PortfolioMaster");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, null, htmlMessage);
            var response = await client.SendEmailAsync(message);
        }
    }

    public class SendGridOptions
    {
        public string ApiKey { get; set; }
    }

}
