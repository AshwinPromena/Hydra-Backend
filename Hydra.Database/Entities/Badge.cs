using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("badge")]
    public class Badge
    {
        public Badge()
        {
            BadgeField = new HashSet<BadgeField>();
            LearnerBadge = new HashSet<LearnerBadge>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("department_id")]
        public long DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        [InverseProperty("Badge")]
        public virtual Department Department { get; set; }

        [Column("issue_date")]
        public DateTime IssueDate { get; set; }

        [Column("expiration_date")]
        public DateTime? ExpirationDate { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("badge_sequence_id")]
        public long? BadgeSequenceId { get; set; }

        [ForeignKey("BadgeSequenceId")]
        [InverseProperty("Badge")]
        public virtual BadgeSequence BadgeSequence { get; set; }

        [Column("image")]
        public string Image { get; set; }

        [Column("approval_user_id")]
        public long? ApprovalUserId { get; set; }

        [ForeignKey("ApprovalUserId")]
        [InverseProperty("Badge")]
        public virtual User ApprovalUser { get; set; }

        [Column("requires_approval")]
        public bool RequiresApproval { get; set; } = false;

        [Column("is_approved")]
        public bool IsApproved { get; set; } = true;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        [Column("badge_type_id")]
        public long BadgeTypeId { get; set; }

        [ForeignKey("BadgeTypeId")]
        [InverseProperty("Badge")]
        public virtual BadgeType BadgeType { get; set; }

        [Column("university_id")]
        public long? UniversityId { get; set; }


        [InverseProperty("Badge")]
        public virtual ICollection<BadgeField> BadgeField { get; set; }

        [InverseProperty("Badge")]
        public virtual ICollection<LearnerBadge> LearnerBadge { get; set; }
    }
}
