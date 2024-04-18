using Microsoft.EntityFrameworkCore;

namespace Hydra.Database.Entities
{
    public class HydraContext : DbContext
    {
        public HydraContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<AccessLevel> AccessLevel { get; set; }
        public virtual DbSet<LearnerBadge> LearnerBadge { get; set; }
        public virtual DbSet<Badge> Badge { get; set; }
        public virtual DbSet<BadgeField> BadgeField { get; set; }
        public virtual DbSet<BadgeApproval> BadgeApproval { get; set; }
        public virtual DbSet<BadgeMedia> BadgeMedia { get; set; }
        public virtual DbSet<BadgeSequence> BadgeSequence { get; set; }
        public virtual DbSet<University> University { get; set; }
    }
}
