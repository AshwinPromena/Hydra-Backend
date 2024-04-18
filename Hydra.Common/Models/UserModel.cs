using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hydra.Common.Models
{
    public class UserModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [JsonProperty("mobileNumber")]
        [Required(ErrorMessage = "Mobile number is required")]
        public string MobileNumber { get; set; }

        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("acccessLevelId")]
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


    public class PasswordResetModel
    {
        [JsonProperty("email")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [JsonProperty("newPassword")]
        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression("^(?!\\s*$).+", ErrorMessage = "New password cannot be empty.")]
        public string NewPassword { get; set; }

        [JsonProperty("confirmPassword")]
        [Required(ErrorMessage = "Confirm password is required.")]
        [RegularExpression("^(?!\\s*$).+", ErrorMessage = "Confirm password cannot be empty.")]
        public string ConfirmPassword { get; set; }
    }
}
