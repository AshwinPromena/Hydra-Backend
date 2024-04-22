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
            Badge = new HashSet<Badge>();
            BadgeMedia = new HashSet<BadgeMedia>();
            BadgeField = new HashSet<BadgeField>();
            LearnerBadge = new HashSet<LearnerBadge>();
            LearnerBadgeIssuedBy = new HashSet<LearnerBadge>();
            BadgeApproval = new HashSet<BadgeApproval>();
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

        [Column("password")]
        [MaxLength(50)]
        public string Password { get; set; }

        [Column("mobile_number")]
        [MaxLength(10)]
        public string MobileNumber { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("access_level_id")]
        public long AccessLevelId { get; set; }

        [ForeignKey("AccessLevelId")]
        [InverseProperty("User")]
        public virtual AccessLevel AccessLevel { get; set; }

        [Column("is_approved")]
        public bool IsApproved { get; set; }

        [Column("department_id")]
        public long DepartmentId { get; set; }

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

        [Column("password_reset_otp")]
        public string PasswordResetOtp { get; set; }

        [Column("otp_expiry_date")]

        public DateTime? OtpExpiryDate { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<UserRole> UserRole { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Badge> Badge { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<BadgeMedia> BadgeMedia { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<BadgeField> BadgeField { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<LearnerBadge> LearnerBadge { get; set; }

        [InverseProperty("IssuedUser")]
        public virtual ICollection<LearnerBadge> LearnerBadgeIssuedBy { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<BadgeApproval> BadgeApproval { get; set; }
    }
}
