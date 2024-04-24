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

        [Column("name")]
        public string Name { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("type_name")]
        public string TypeName { get; set; }
    }
}
