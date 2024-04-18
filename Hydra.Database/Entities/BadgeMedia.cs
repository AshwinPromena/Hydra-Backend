using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("badge_media")]
    public class BadgeMedia
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("badge_id")]
        public long BadgeId { get; set; }

        [ForeignKey("BadgeId")]
        [InverseProperty("BadgeMedia")]
        public virtual Badge Badge { get; set; }

        [Column("badge_image_url")]
        public string BadgeImageUrl { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BadgeMedia")]
        public virtual User User { get; set; }
    }
}
