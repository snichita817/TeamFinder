using TeamFinder.Models.DTO.Auth;

namespace TeamFinder.Repositories.Interface
{
    public interface IEmailRepository
    {
        Task<bool> SendEmailAsync(EmailSendDto emailSend);
    }
}
