using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("user_role")]
    public class UserRole
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("UserRole")]
        public virtual User User { get; set; }

        [Column("role_id")]
        public long RoleId { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("UserRole")]
        public virtual Role Role { get; set; }
    }
}
