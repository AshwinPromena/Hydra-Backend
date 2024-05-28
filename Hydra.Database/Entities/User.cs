using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("user")]
    public class User
    {
        public User()
        {
            UserRole = new HashSet<UserRole>();
            LearnerBadge = new HashSet<LearnerBadge>();
            LearnerBadgeIssuedBy = new HashSet<LearnerBadge>();
            Badge = new HashSet<Badge>();
            Verification = new HashSet<Verification>();
            PasswordResetToken = new HashSet<PasswordResetToken>();
            DeletedLearner = new HashSet<DeletedLearner>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_name")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Column("email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Column("email_2")]
        [MaxLength(50)]
        public string Email2 { get; set; }

        [Column("email_3")]
        [MaxLength(50)]
        public string Email3 { get; set; }

        [Column("password")]
        [MaxLength(50)]
        public string Password { get; set; }

        [Column("mobile_number")]
        [MaxLength(10)]
        public string MobileNumber { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("access_level_id")]
        public long? AccessLevelId { get; set; }

        [ForeignKey("AccessLevelId")]
        [InverseProperty("User")]
        public virtual AccessLevel AccessLevel { get; set; }

        [Column("is_approved")]
        public bool IsApproved { get; set; }

        [Column("department_id")]
        public long? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        [InverseProperty("User")]
        public virtual Department Department { get; set; }

        [Column("profile_picture")]
        public string ProfilePicture { get; set; }

        [Column("is_archived")]
        public bool IsArchived { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("learner_id")]
        public long LearnerId { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        [InverseProperty("User")]
        public virtual ICollection<UserRole> UserRole { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<LearnerBadge> LearnerBadge { get; set; }

        [InverseProperty("IssuedUser")]
        public virtual ICollection<LearnerBadge> LearnerBadgeIssuedBy { get; set; }

        [InverseProperty("ApprovalUser")]
        public virtual ICollection<Badge> Badge { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Verification> Verification { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<PasswordResetToken> PasswordResetToken { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<DeletedLearner> DeletedLearner { get; set; }
    }
}
