using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;

namespace Hydra.DatbaseLayer.Repository
{
    public class UserRepository(HydraContext context) : CommonRepository<User>(context), IUserRepository
    {
        private readonly HydraContext _context = context;
    }
}
