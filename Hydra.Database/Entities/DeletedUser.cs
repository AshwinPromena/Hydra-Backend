﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("deleted_user")]
    public class DeletedUser
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("deleted_user_id")]
        public long DeletedUserId { get; set; }

        [ForeignKey("DeletedUserId")]
        [InverseProperty("DeletedUser")]
        public virtual User User { get; set; }

        [Column("deleted_user_name")]
        public string DeletedUserName { get; set; }

        [Column("deleted_user_email")]
        public string DeletedUserEmail { get; set; }

        [Column("reason")]
        public string Reason { get; set; }

        [Column("deleted_date")]
        public DateTime DeletedDate { get; set; }

        [Column("deleted_user_university_id")]
        public long DeletedUserUniversityId { get; set; }
    }
}
