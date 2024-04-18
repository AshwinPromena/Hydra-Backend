using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.DatbaseLayer.Repository
{
    public class UserRoleRepository(HydraContext context) : CommonRepository<UserRole>(context), IUserRoleRepository
    {
        private readonly HydraContext _context = context;
    }
}
