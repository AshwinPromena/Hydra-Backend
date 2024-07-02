using System.Data;

namespace Hydra.Common.Globle
{
    public class TemplateConstatnt
    {
        public const string PasswordResetOtpTemplate = "PasswordResetOtp.html";
        public const string PasswordResetLink = "PasswordResetLink.html";
        public const string ContactSupport = "ContactSupport.html";
        public const string StaffLoginCredentialTemplate = "StaffLoginCredential.html";
    }

    public class TemplateSubjectConstant
    {
        public const string PasswordResetOtpTemplateSubject = "Password Reset OTP";
        public const string PasswordResetLink = "Password Reset Link";
        public const string ContactSupport = "New Contact Form Submission";
        public const string StaffLoginCredentialSubject = "Your Login Credentials for Badge Factory";
    }

    public class ReplaceStringConstant
    {
        public const string UserName = "{UserName}";
        public const string Otp = "{OTP}";
        public const string Link = "{Link}";
        public const string Name = "{Name}";
        public const string Email = "{Email}";
        public const string MobileNumber = "{MobileNumber}";
        public const string Description = "{Description}";
        public const string Password = "{Password}";
    }
}
