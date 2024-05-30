using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hydra.Common.Models
{
    public class UserRegisterModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("userName")]
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [JsonProperty("mobileNumber")]
        [Required(ErrorMessage = "Mobile number is required")]
        public string MobileNumber { get; set; }

        [JsonProperty("departmentId")]
        [Required(ErrorMessage = "Department is required")]
        public long DepartmentId { get; set; }

        [JsonProperty("acccessLevelId")]
        [Required(ErrorMessage = "Access level is required")]
        public long AccessLevelId { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^(?!\\s*$).+", ErrorMessage = "password cannot be empty.")]
        public string Password { get; set; }
    }

    public class LoginModel
    {
        [JsonProperty("userName")]
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^(?!\\s*$).+", ErrorMessage = "password cannot be empty.")]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }

    public class ForgotPasswordModel
    {
        [JsonProperty("userName")]
        [Required(ErrorMessage = "UesrName is required.")]
        public string UserName { get; set; }
    }

    public class ResetPasswordModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^(?!\\s*$).+", ErrorMessage = "New password cannot be empty.")]
        public string Password { get; set; }
    }

    public class ValidateOtpModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("otp")]
        public string Otp { get; set; }
    }

    public class ChangePasswordModel
    {
        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
    }
}
