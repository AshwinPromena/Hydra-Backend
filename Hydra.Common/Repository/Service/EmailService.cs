using Hydra.Common.Globle;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Hydra.Common.Repository.Service
{
    public class EmailService(IConfiguration configuration, IHostEnvironment environment) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IHostEnvironment _environment = environment;

        public async Task<ApiResponse> SendPasswordResetLink(string email, long userId, string userName, string token)
        {
            string Link = $"https://hydra-react.vercel.app/otpverification?userId={userId}&token={token}";
            string template = await ReadTemplate(TemplateConstatnt.PasswordResetLink);
            string content = template.Replace(ReplaceStringConstant.Link, Link)
                                     .Replace(ReplaceStringConstant.UserName, userName);
            await SendMail(email, TemplateSubjectConstant.PasswordResetLink, content);
            return new(200, ResponseConstants.Success);
        }

        public async Task<string> SendPasswordResetOTP(string email, string userName)
        {
            string Otp = GenerateOtp(4);
            string template = await ReadTemplate(TemplateConstatnt.PasswordResetOtpTemplate);
            string content = template.Replace(ReplaceStringConstant.Otp, Otp)
                                     .Replace(ReplaceStringConstant.UserName, userName);
            await SendMail(email, TemplateSubjectConstant.PasswordResetOtpTemplateSubject, content);
            return Otp;
        }

        public async Task<ApiResponse> SendContactSupportMail(ContactSupportModel model)
        {
            string template = await ReadTemplate(TemplateConstatnt.ContactSupport);
            string content = template.Replace(ReplaceStringConstant.Name, model.Name)
                                     .Replace(ReplaceStringConstant.Email, model.Email)
                                     .Replace(ReplaceStringConstant.MobileNumber, model.MobileNumber)
                                     .Replace(ReplaceStringConstant.Description, model.Description);

            var email = "hydra@yopmail.com";
            await SendMail(email, TemplateSubjectConstant.ContactSupport, content);
            return new(200, ResponseConstants.Success);
        }

        public async Task<ApiResponse> SendStaffLoginCredential(string email, string name, string userName, string password)
        {
            string link = $"https://bfactory-react.vercel.app";
            string template = await ReadTemplate(TemplateConstatnt.StaffLoginCredentialTemplate);
            string content = template.Replace(ReplaceStringConstant.Link, link)
                                     .Replace(ReplaceStringConstant.UserName, userName)
                                     .Replace(ReplaceStringConstant.Password, password)
                                     .Replace(ReplaceStringConstant.Name, name);
            await SendMail(email, TemplateSubjectConstant.StaffLoginCredentialSubject, content);
            return new(200, ResponseConstants.Success);
        }

        public async Task<string> ReadTemplate(string templateName)
        {
            var pathToFile = $"{_environment.ContentRootPath}{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{templateName}";
            string builder = "";
            using (StreamReader reader = File.OpenText(pathToFile))
            {
                builder = await reader.ReadToEndAsync();
            }
            return builder;
        }

        public async Task<ApiResponse> SendMail(string email, string subject, string content, List<string> attachments = null)
        {
            try
            {
                using var client = new SmtpClient(_configuration["EmailConfiguration:SmtpServer"], int.Parse
                                                (_configuration["EmailConfiguration:Port"]))
                {
                    Credentials = new NetworkCredential(_configuration["EmailConfiguration:UserName"],
                                                     _configuration["EmailConfiguration:Password"]),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                };
                using var mailmessage = new MailMessage();
                mailmessage.From = new MailAddress(_configuration["EmailConfiguration:From"]);
                mailmessage.To.Insert(0, new MailAddress(email));
                mailmessage.Subject = subject;
                mailmessage.Body = content;
                mailmessage.IsBodyHtml = true;
                if (attachments != null)
                {
                    attachments.ForEach(attachment =>
                    {
                        mailmessage.Attachments.Add(new Attachment(attachment));
                    });
                }

                await client.SendMailAsync(mailmessage);

                return new(200, "Email Sent Successfully");
            }
            catch (Exception ex)
            {
                return new(500, ex.Message);
            }
        }

        public string GenerateOtp(int Length)
        {
            const string chars = "0123456789";
            Random rnd = new();
            StringBuilder stringBuilder = new(Length);
            for (int i = 0; i < Length; i++)
            {
                int index = rnd.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }
            return stringBuilder.ToString();
        }
    }
}
