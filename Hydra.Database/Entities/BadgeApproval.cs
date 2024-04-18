using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("badge_approval")]
    public class BadgeApproval
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("badge_id")]
        public long BadgeId { get; set; }

        [ForeignKey("BadgeId")]
        [InverseProperty("BadgeApproval")]
        public virtual Badge Badge { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BadgeApproval")]
        public virtual User User { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [Column("is_approved")]
        public bool IsApproved { get; set; }
    }
}
