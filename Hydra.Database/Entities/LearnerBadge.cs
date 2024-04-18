using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("learner_badge")]
    public class LearnerBadge
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("LearnerBadge")]
        public virtual User User { get; set; }

        [Column("badge_id")]
        public long BadgeId { get; set; }

        [ForeignKey("BadgeId")]
        [InverseProperty("LearnerBadge")]
        public virtual Badge Badge { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("issued_by")]
        public long IssuedBy { get; set; }

        [ForeignKey("IssuedBy")]
        [InverseProperty("LearnerBadgeIssuedBy")]
        public virtual User IssuedUser { get; set; }

        [Column("is_revoked")]
        public bool IsRevoked { get; set; }
    }
}
