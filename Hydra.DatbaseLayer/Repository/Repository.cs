using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.DatbaseLayer.Repository
{
    public class AccessLevelRepository(HydraContext context) : CommonRepository<AccessLevel>(context), IAccessLevelRepository { }

    public class BadgeRepository(HydraContext context) : CommonRepository<Badge>(context), IBadgeRepository { }

    public class BadgeFieldRepository(HydraContext context) : CommonRepository<BadgeField>(context), IBadgeFieldRepository { }

    public class LearnerBadgeRepository(HydraContext context) : CommonRepository<LearnerBadge>(context), ILearnerBadgeRepository { }

    public class BadgeSequenceRepository(HydraContext context) : CommonRepository<BadgeSequence>(context), IBadgeSequenceRepository { }

    public class DepartmentRepository(HydraContext context) : CommonRepository<Department>(context), IDepartmentRepository { }

    public class LearnerRepository(HydraContext context) : CommonRepository<LearnerBadge>(context), ILearnerRepository { }

    public class RoleRepository(HydraContext context) : CommonRepository<Role>(context), IRoleRepository { }

    public class UserRepository(HydraContext context) : CommonRepository<User>(context), IUserRepository { }

    public class UserRoleRepository(HydraContext context) : CommonRepository<UserRole>(context), IUserRoleRepository { }

    public class VerificationRepository(HydraContext context) : CommonRepository<Verification>(context), IVerificationRepository { }

    public class PasswordResetTokenRepository(HydraContext context) : CommonRepository<PasswordResetToken>(context), IPasswordResetTokenRepository { }

    public class DeletedUserRepository(HydraContext context) : CommonRepository<DeletedUser>(context), IDeletedUserRepository { }

    public class BadgeTypeRepository(HydraContext context) : CommonRepository<BadgeType>(context) , IBadgeTypeRepository { }

    public class ContactSupportFormRepository(HydraContext context) : CommonRepository<ContactSupportForm>(context), IContactSupportFormRepository { }
}
