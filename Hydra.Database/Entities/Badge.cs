using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("badge")]
    public class Badge
    {
        public Badge()
        {
            BadgeMedia = new HashSet<BadgeMedia>();
            BadgeField = new HashSet<BadgeField>();
            LearnerBadge = new HashSet<LearnerBadge>();
            BadgeApproval = new HashSet<BadgeApproval>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("badge_name")]
        public string BadgeName { get; set; }

        [Column("department_id")]
        public long DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        [InverseProperty("Badge")]
        public virtual Department Department { get; set; }

        [Column("issue_date")]
        public DateTime IssueDate { get; set; }

        [Column("expiration_date")]
        public DateTime ExpirationDate { get; set; }

        [Column("badge_description")]
        public string BadgeDescription { get; set; }

        [Column("sequence_id")]
        public long SequenceId { get; set; }

        [ForeignKey("SequenceId")]
        [InverseProperty("Badge")]
        public virtual BadgeSequence BadgeSequence { get; set; }

        [Column("requires_approval")]
        public bool RequiresApproval { get; set; }

        [Column("is_approved")]
        public bool IsApproved { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Badge")]
        public virtual User User { get; set; }

        [InverseProperty("Badge")]
        public virtual ICollection<BadgeMedia> BadgeMedia { get; set; }

        [InverseProperty("Badge")]
        public virtual ICollection<BadgeField> BadgeField { get; set; }

        [InverseProperty("Badge")]
        public virtual ICollection<LearnerBadge> LearnerBadge { get; set; }

        [InverseProperty("Badge")]
        public virtual ICollection<BadgeApproval> BadgeApproval { get; set; }
    }
}
