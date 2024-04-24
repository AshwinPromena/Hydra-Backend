using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.DatbaseLayer.Repository
{
    public class BadgeRepository(HydraContext context) : CommonRepository<Badge>(context) ,IBadgeRepository
    {
        private readonly HydraContext _context = context;
    }
}
