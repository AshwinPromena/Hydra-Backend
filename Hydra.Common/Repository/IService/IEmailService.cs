﻿using Hydra.Common.Models;
namespace Hydra.Common.Repository.IService
{
    public interface IEmailService
    {
        Task<ApiResponse> SendPasswordResetLink(string email, long userId, string userName, string token);

        Task<string> SendPasswordResetOTP(string email, string userName);

        Task<ApiResponse> SendContactSupportMail(ContactSupportModel model);

        Task<ApiResponse> SendMail(string email, string subject, string content, List<string> attachments = null);

        Task<ApiResponse> SendStaffLoginCredential(string email, string name, string userName, string password);

        Task<string> ReadTemplate(string templateName);
    }
}
