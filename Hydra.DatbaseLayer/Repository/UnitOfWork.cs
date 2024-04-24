using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.DatbaseLayer.Repository
{
    public class UnitOfWork(HydraContext context) : IUnitOfWork
    {
        private readonly HydraContext _context = context;

        public IUserRepository UserRepository => new UserRepository(_context);

        public IRoleRepository RoleRepository => new RoleRepository(_context);

        public IUserRoleRepository UserRoleRepository => new UserRoleRepository(_context);

        public ILearnerRepository LearnerRepository => new LearnerRepository(_context);

        public IAccessLevelRepository AccessLevelRepository => new AccessLevelRepository(_context);

        public IDepartmentRepository DepartmentRepository => new DepartmentRepository(_context);

        public IBadgeSequenceRepository BadgeSequenceRepository => new BadgeSequenceRepository(_context);

        public IBadgeRepository BadgeRepository => new BadgeRepository(_context);
    }
}
