using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.DatbaseLayer.Repository
{
    public class DepartmentRepository(HydraContext context) : CommonRepository<Department>(context), IDepartmentRepository
    {
        private readonly HydraContext _context = context;
    }
}
