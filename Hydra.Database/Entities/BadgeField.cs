using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("badge_field")]
    public class BadgeField
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("badge_id")]
        public long BadgeId { get; set; }

        [ForeignKey("BadgeId")]
        [InverseProperty("BadgeField")]
        public virtual Badge Badge { get; set; }

        [Column("field_content")]
        public string FieldContent { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("type_name")]
        public string TypeName { get; set; }

        [Column("field_name")]
        public string FieldName { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BadgeField")]
        public virtual User User { get; set; }
    }
}
