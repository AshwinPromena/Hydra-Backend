using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hydra.Common.Models
{
    public class AddStaffModel
    {
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
        [Required(ErrorMessage = "Department is required.")]
        public long? DepartmentId { get; set; }

        [JsonProperty("acccessLevelId")]
        [Required(ErrorMessage = "Access level is required.")]
        public long? AccessLevelId { get; set; }

        [JsonProperty("firstName")]
        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }
    }

    public class UpdateStaffModel : AddStaffModel
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }
    }

    public class DeleteStaffModel
    {
        [JsonProperty("userIds")]
        public List<long> UserIds { get; set; }
    }

    public class GetStaffModel : UpdateStaffModel
    {
        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("isApproved")]
        public bool IsApproved { get; set; }

        [JsonProperty("isArchived")]
        public bool IsArchived { get; set; }

        [JsonProperty("accessLevelName")]
        public string AccessLevelName { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
    }
}
