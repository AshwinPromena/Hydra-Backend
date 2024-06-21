using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("badge_type")]
    public class BadgeType
    {
        public BadgeType()
        {
            Badge = new HashSet<Badge>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }


        [InverseProperty("BadgeType")]
        public virtual ICollection<Badge> Badge { get; set; }
    }
}
