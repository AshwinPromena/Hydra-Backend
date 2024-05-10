using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("passsword_reset_token")]
    public class PasswordResetToken
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("PasswordResetToken")]
        public virtual User User { get; set; }

        [Column("reset_token")]
        public string ResetToken { get; set; }

        [Column("token_expiry_date")]
        public DateTime TokenExpiryDate { get; set; }

        [Column("is_token_active")]
        public bool IsTokenActive { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

    }
}
