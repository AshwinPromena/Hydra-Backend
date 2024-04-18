using Hydra.Common.Models;
namespace Hydra.Common.Repository.IService
{
    public interface IEmailService
    {
        Task<string> PasswordResetTemplate(string email);

        Task<ApiResponse> SendMail(string email, string subject, string content, List<string> attachments = null);

        Task<string> ReadTemplate(string templateName);
    }
}
