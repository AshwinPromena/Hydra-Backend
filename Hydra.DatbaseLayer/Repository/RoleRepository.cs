using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.DatbaseLayer.Repository
{
    public class RoleRepository(HydraContext context) : CommonRepository<Role>(context), IRoleRepository
    {
        private readonly HydraContext _context = context;
    }
}
