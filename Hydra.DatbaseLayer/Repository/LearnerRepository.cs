using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.DatbaseLayer.Repository
{
    public class LearnerRepository(HydraContext context) : CommonRepository<LearnerBadge>(context), ILearnerRepository
    {
        private readonly HydraContext _context = context;
    }
}
