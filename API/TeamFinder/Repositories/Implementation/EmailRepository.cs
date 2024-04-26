using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using TeamFinder.Models.DTO.Auth;
using TeamFinder.Repositories.Interface;

namespace TeamFinder.Repositories.Implementation
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;
        public EmailRepository(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(EmailSendDto emailSend)
        {
            MailjetClient client = new MailjetClient(_configuration["Mailjet:ApiKey"], _configuration["Mailjet:SecretKey"]);

            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_configuration["Email:From"], _configuration["Email:ApplicationName"]))
                .WithSubject(emailSend.Subject)
                .WithHtmlPart(emailSend.Body)
                .WithTo(new SendContact(emailSend.To))
                .Build();

            var response = await client.SendTransactionalEmailAsync(email);

            if(response.Messages != null)
            {
                if (response.Messages[0].Status == "success")
                {
                    return true;
                }

            }

            return false;
        }
    }
}
