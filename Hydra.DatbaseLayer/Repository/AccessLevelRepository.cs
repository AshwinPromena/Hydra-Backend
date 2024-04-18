using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.DatbaseLayer.Repository
{
    public class AccessLevelRepository(HydraContext context) : CommonRepository<AccessLevel>(context), IAccessLevelRepository
    {
        private readonly HydraContext _context = context;
    }
}
