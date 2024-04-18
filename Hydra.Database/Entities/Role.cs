using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("role")]
    public class Role
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
