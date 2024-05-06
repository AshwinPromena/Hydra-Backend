using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("verification")]
    public class Verification
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Verification")]
        public virtual User User { get; set; }

        [Column("otp")]
        [MaxLength(6)]
        public string OTP {  get; set; }

        [Column("otp_expiry_date")]
        public DateTime OtpExpiryDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("password_reset_token")]
        public string PasswordResetToken {  get; set; }

        [Column("password_reser_token_expiry_date")]
        public DateTime PasswordResetTokenExpiryDate { get; set; }

        [Column("is_token_active")]
        public bool IsTokenActive { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
