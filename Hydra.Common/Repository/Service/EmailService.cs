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


        public async Task<string> PasswordResetTemplate(string email)
        {
            string Otp = GenerateOtp(4);
            string template = await ReadTemplate("PasswordResetOtp.html");
            string content = template.Replace("{Otp}", Otp);
            await SendMail(email, "OTP to Reset You Password", content);
            return Otp;
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


        public async Task<string> ReadTemplate(string templateName)
        {
            var pathToFile = $"{_environment.ContentRootPath}{Path.DirectorySeparatorChar}EmailTemplates{Path.DirectorySeparatorChar}{templateName}";
            string builder = "";
            using (StreamReader reader = File.OpenText(pathToFile))
            {
                builder = await reader.ReadToEndAsync();
            }
            return builder;
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
