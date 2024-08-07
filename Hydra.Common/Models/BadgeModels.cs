﻿using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class BadgeFactoryDashBoardModel
    {
        [JsonProperty("totalCredentials")]
        public int TotalCredentials { get; set; }

        [JsonProperty("activeCredentials")]
        public int ActiveCredentials { get; set; }

        [JsonProperty("recentCredentials")]
        public int RecentCredentials { get; set; }

        [JsonProperty("totalLearnerCount")]
        public int TotalLearnerCount { get; set; }

        [JsonProperty("learnerWithBadge")]
        public int LearnerWithBadge { get; set; }

        [JsonProperty("learnerWithoutBadge")]
        public int LearnerWithoutBadge { get; set; }

        [JsonProperty("pendingApproval")]
        public int PendingApproval { get; set; }

        [JsonProperty("recentAssignedCredentials")]
        public int RecentAssignedCredentials { get; set; }

        [JsonProperty("badgeCount")]
        public int BadgeCount { get; set; }

        [JsonProperty("certificateCount")]
        public int CertificateCount {  get; set; }

        [JsonProperty("licenseCount")]
        public int LicenseCount { get; set; }

        [JsonProperty("miscellaneousCount")]
        public int MiscellaneousCount {  get; set; }

        [JsonProperty("learnerBadgeCount")]
        public int LearnerBadgeCount { get; set; }

        [JsonProperty("learnerCertificateCount")]
        public int LearnerCertificateCount { get; set; }

        [JsonProperty("learnerLicenseCount")]
        public int LearnerLicenseCount { get; set; }

        [JsonProperty("learnerMiscellaneousCount")]
        public int LearnerMiscellaneousCount { get; set; }
    }

    public class AddBadgeModel
    {
        [JsonProperty("badgeName")]
        public string BadgeName { get; set; }

        [JsonProperty("badgeDescription")]
        public string BadgeDescription { get; set; }

        [JsonProperty("issueDate")]
        public DateTime IssueDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime? ExpirationDate { get; set; }

        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("badgeImage")]
        public string BadgeImage { get; set; }

        [JsonProperty("badgeSequenceId")]
        public long? BadgeSequenceId { get; set; }

        [JsonProperty("isRequiresApproval")]
        public bool IsRequiresApproval { get; set; } = false;

        [JsonProperty("approvalUserId")]
        public long? ApprovalUserId { get; set; }

        [JsonProperty("learningOutcomes")]
        public List<BadgeFieldModel> LearningOutcomes { get; set; }

        [JsonProperty("competencies")]
        public List<BadgeFieldModel> Competencies { get; set; }

        [JsonProperty("badgeTypeId")]
        public long BadgeTypeId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
    }

    public class BadgeFieldModel
    {
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        [JsonProperty("fieldContent")]
        public string FieldContent { get; set; }
    }

    public class UpdateBadgeModel : AddBadgeModel
    {
        [JsonProperty("badgeId")]
        public long BadgeId { get; set; }
    }

    public class DeleteBadgeModel
    {
        [JsonProperty("badgeIds")]
        public List<long> BadgeIds { get; set; }
    }

    public class GetBadgeModel : UpdateBadgeModel
    {
        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }

        [JsonProperty("badgeSequenceName")]
        public string BadgeSequenceName { get; set; }

        [JsonProperty("approvalUser")]
        public string ApprovalUser { get; set; }

        [JsonProperty("isSequence")]
        public bool IsSequence { get; set; }

        [JsonProperty("isApproved")]
        public bool IsApproved { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("badgeTypeName")]
        public string BadgeTypeName { get; set; }
    }

    public class AssignBadgeModel
    {
        [JsonProperty("userIds")]
        public List<long> UserIds { get; set; }

        [JsonProperty("badges")]
        public List<long> BadgeIds { get; set; }
    }

    public class BadgeSequenceOutputModel
    {
        [JsonProperty("sequenceId")]
        public long SequenceId { get; set; }

        [JsonProperty("sequenceName")]
        public string SequenceName { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }

    public class DepartmentOutputModel
    {
        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }

    public class ApproveBadgeModel
    {
        [JsonProperty("badgeIds")]
        public List<long> BadgeIds { get; set; }
    }

    public class NotApprovedBadgeModel
    {
        [JsonProperty("badgeId")]
        public long BadgeId { get; set; }

        [JsonProperty("badgeName")]
        public string BadgeName { get; set; }
    }

    public class GetUnApprovedBadgeInputModel : PagedResponseInput
    {
        [JsonProperty("sortBy")]
        public int SortBy { get; set; }
    }

    public class GetAllBadgeInputModel : PagedResponseInput
    {
        [JsonProperty("sortBy")]
        public int SortBy { get; set; }
    }

    public class GetBadgePicturesModel
    {
        [JsonProperty("badgePictureUrl")]
        public string BadgePictureUrl { get; set; }
    }


    public class DepartmentIDModel
    {
        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }
    }
}
