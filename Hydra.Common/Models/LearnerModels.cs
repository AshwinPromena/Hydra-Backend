using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Hydra.Common.Models
{
    public class GetS3UrlModel
    {
        [JsonProperty("s3Url")]
        public string S3Url { get; set; }
    }

    public class AddLearnerModel
    {
        [JsonProperty("firstName")]
        [Required(ErrorMessage = "This field is required")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        [Required(ErrorMessage = "This field is required")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [JsonProperty("email2")]
        public string Email2 { get; set; }

        [JsonProperty("email3")]
        public string Email3 { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("learnerId")]
        public long LearnerId { get; set; } = 0;
    }

    public class ExistingLearnerModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class AssignBadgeToLearnerModel
    {
        [JsonProperty("userIds")]
        public List<long> UserIds { get; set; }

        [JsonProperty("badgeIds")]
        public List<long> BadgeIds { get; set; }
    }

    public class RemoveBadgeModel
    {
        [JsonProperty("userIds")]
        public List<long> UserIds { get; set; }

        [JsonProperty("badgeIds")]
        public List<long> BadgeIds { get; set; }
    }

    public class LearnerDashBoardModel
    {
        [JsonProperty("learnerInTotal")]
        public int LearnerInTotal { get; set; }

        [JsonProperty("learnerWithBadge")]
        public int LearnerWithBadge { get; set; }

        [JsonProperty("learnerWithoutBadge")]
        public int LearnerWithoutBadge { get; set; }

        [JsonProperty("addedTodayCount")]
        public int AddedTodayCount { get; set; }

        [JsonProperty("learnerBadgeCount")]
        public int LearnerBadgeCount { get; set; }

        [JsonProperty("learnerCertificateCount")]
        public int LearnerCertificateCount { get; set; }

        [JsonProperty("learnerLicenseCount")]
        public int LearnerLicenseCount { get; set; }

        [JsonProperty("learnerMiscellaneousCount")]
        public int LearnerMiscellaneousCount { get; set; }
    }

    public class GetAllLearnerInputModel : PagedResponseInput
    {
        [JsonProperty("fromDate")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("toDate")]
        public DateTime? ToDate { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }

    public class GetLearnerModel
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email2")]
        public string Email2 { get; set; }

        [JsonProperty("email3")]
        public string Email3 { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty("learnerBadgeModel")]
        public List<LearnerBadgeModel> LearnerBadgeModel { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("learnerId")]
        public long LearnerId { get; set; }
    }

    public class GetLearnerByIdModel
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email2")]
        public string Email2 { get; set; }

        [JsonProperty("email3")]
        public string Email3 { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty("learnerBadgeModel")]
        public List<LearnerBadgeModel> LearnerBadgeModel { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("learnerId")]
        public long LearnerId { get; set; }

        [JsonProperty("active")]
        public int Active { get; set; }

        [JsonProperty("expiring")]
        public int Expiring { get; set; }

        [JsonProperty("expired")]
        public int Expired { get; set; }

        [JsonProperty("getActiveCredentialModel")]
        public List<GetActiveCredentialModel> GetActiveCredentialModel { get; set; }

        [JsonProperty("getExpirinyCredentialModel")]
        public List<GetExpirinyCredentialModel> GetExpirinyCredentialModel { get; set; }

        [JsonProperty("getexpiredCredentialModel")]
        public List<GetexpiredCredentialModel> GetexpiredCredentialModel { get; set; }
    }

    public class GetActiveCredentialModel
    {
        [JsonProperty("badgeId")]
        public long BadgeId { get; set; }

        [JsonProperty("badgeName")]
        public string BadgeName { get; set; }

        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("issuedDate")]
        public DateTime IssuedDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("sequenceId")]
        public long? SequenceId { get; set; }

        [JsonProperty("sequenceName")]
        public string SequenceName { get; set; }

        [JsonProperty("badgeTypeId")]
        public long BadgeTypeId { get; set; }

        [JsonProperty("badgeTypeName")]
        public string BadgeTypeName { get; set; }
    }

    public class GetExpirinyCredentialModel
    {
        [JsonProperty("badgeId")]
        public long BadgeId { get; set; }

        [JsonProperty("badgeName")]
        public string BadgeName { get; set; }

        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("issuedDate")]
        public DateTime IssuedDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("sequenceId")]
        public long? SequenceId { get; set; }

        [JsonProperty("sequenceName")]
        public string SequenceName { get; set; }

        [JsonProperty("badgeTypeId")]
        public long BadgeTypeId { get; set; }

        [JsonProperty("badgeTypeName")]
        public string BadgeTypeName { get; set; }
    }

    public class GetexpiredCredentialModel
    {
        [JsonProperty("badgeId")]
        public long BadgeId { get; set; }

        [JsonProperty("badgeName")]
        public string BadgeName { get; set; }

        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("issuedDate")]
        public DateTime IssuedDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("sequenceId")]
        public long? SequenceId { get; set; }

        [JsonProperty("sequenceName")]
        public string SequenceName { get; set; }

        [JsonProperty("badgeTypeId")]
        public long BadgeTypeId { get; set; }

        [JsonProperty("badgeTypeName")]
        public string BadgeTypeName { get; set; }
    }

    public class UpdateLearnerModel
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("firstName")]
        [Required(ErrorMessage = "This field is required")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        [Required(ErrorMessage = "This field is required")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [JsonProperty("email2")]
        public string Email2 { get; set; }

        [JsonProperty("email3")]
        public string Email3 { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("learnerId")]
        public long LearnerId { get; set; }
    }

    public class LearnerBadgeModel
    {
        [JsonProperty("badgeId")]
        public long BadgeId { get; set; }

        [JsonProperty("badgeName")]
        public string BadgeName { get; set; }

        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("issuedDate")]
        public DateTime IssuedDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("sequenceId")]
        public long? SequenceId { get; set; }

        [JsonProperty("sequenceName")]
        public string SequenceName { get; set; }

        [JsonProperty("badgeTypeId")]
        public long BadgeTypeId { get; set; }

        [JsonProperty("badgeTypeName")]
        public string BadgeTypeName { get; set; }
    }

    public class RevokeBadgeModel
    {
        [JsonProperty("userIds")]
        public List<long> UserIds { get; set; }

        [JsonProperty("badgeIds")]
        public List<long> BadgeIds { get; set; }
    }

    public class RemoveLearnerModel
    {
        [JsonProperty("userIds")]
        public long UserIds { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
