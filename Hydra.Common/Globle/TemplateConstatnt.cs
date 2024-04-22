using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Common.Globle
{
    public class TemplateConstatnt
    {
        public const string PasswordResetOtpTemplate = "PasswordResetOtp.html";
    }

    public class TemplateSubjectConstant
    {
        public const string PasswordResetOtpTemplateSubject = "Password Reset OTP";
    }

    public class ReplaceStringConstant
    {
        public const string UserName = "{UserName}";
        public const string Otp = "{OTP}";
    }
}
