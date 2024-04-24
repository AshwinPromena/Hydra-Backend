using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Common.Models
{
    public class AddBadgeModel
    {
        [JsonProperty("badgeName")]
        public string BadgeName { get; set; }

        [JsonProperty("badgeDescription")]
        public string BadgeDescription { get; set; }

        [JsonProperty("issueDate")]
        public DateTime IssueDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; set; }

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
    }

    public class AssignBadgeModel
    {
        [JsonProperty("userIds")]
        public List<long> UserIds { get; set; }

        [JsonProperty("badges")]
        public List<long> BadgeIds { get; set;}
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
}
