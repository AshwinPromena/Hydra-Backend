using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class VerificationRepository(HydraContext context) : CommonRepository<Verification>(context) , IVerificationRepository { }
}
