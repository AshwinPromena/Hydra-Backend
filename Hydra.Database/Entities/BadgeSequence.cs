using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("badge_sequence")]
    public class BadgeSequence
    {
        public BadgeSequence()
        {
            Badge = new HashSet<Badge>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("sequence_name")]
        public string SequenceName { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BadgeSequence")]
        public virtual User User { get; set; }

        [InverseProperty("BadgeSequence")]
        public virtual ICollection<Badge> Badge { get; set; }
    }
}
