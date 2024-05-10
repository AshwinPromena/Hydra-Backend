using Hydra.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.DatbaseLayer.IRepository
{
    public interface IAccessLevelRepository : ICommonRepository<AccessLevel> { }

    public interface IBadgeRepository : ICommonRepository<Badge> { }
    
    public interface IBadgeFieldRepository : ICommonRepository<BadgeField> { }

    public interface ILearnerBadgeRepository : ICommonRepository<LearnerBadge> { }

    public interface IBadgeSequenceRepository : ICommonRepository<BadgeSequence> { }

    public interface IDepartmentRepository : ICommonRepository<Department> { }

    public interface ILearnerRepository : ICommonRepository<LearnerBadge> { }

    public interface IRoleRepository : ICommonRepository<Role> { }

    public interface IUserRepository : ICommonRepository<User> { }

    public interface IUserRoleRepository : ICommonRepository<UserRole> { }

    public interface IVerificationRepository : ICommonRepository<Verification> { }

    public interface IPasswordResetTokenRepository : ICommonRepository<PasswordResetToken> { }
}
