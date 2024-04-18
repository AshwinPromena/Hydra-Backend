using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("access_level")]
    public class AccessLevel
    {
        public AccessLevel()
        {
            User = new HashSet<User>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("access_level_name")]
        public string AccessLevelName { get; set; }


        [InverseProperty("AccessLevel")]
        public virtual ICollection<User> User { get; set; }
    }
}
