using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("department")]
    public class Department
    {
        public Department()
        {
            User = new HashSet<User>();
            Badge = new HashSet<Badge>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("department_name")]
        [MaxLength(100)]
        public string DepartmentName { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [InverseProperty("Department")]
        public virtual ICollection<User> User { get; set; }

        [InverseProperty("Department")]
        public virtual ICollection<Badge> Badge { get; set; }
    }
}
