using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydra.Database.Entities
{
    [Table("deleted_learner")]
    public class DeletedLearner
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("learner_id")]
        public long LearnerId { get; set; }

        [ForeignKey("LearnerId")]
        [InverseProperty("DeletedLearner")]
        public virtual User User { get; set; }

        [Column("reason")]
        public string Reason { get; set; }

        [Column("deleted_date")]
        public DateTime DeleteDate { get; set; }
    }
}
