using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Database.Entities
{
    [Table("contact_support_form")]
    public class ContactSupportForm
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Column("email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Column("mobile_number")]
        [MaxLength(20)]
        public string MobileNumber { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
