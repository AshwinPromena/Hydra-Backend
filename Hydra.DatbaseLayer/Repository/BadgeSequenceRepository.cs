using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.DatbaseLayer.Repository
{
    public class BadgeSequenceRepository(HydraContext context) : CommonRepository<BadgeSequence>(context), IBadgeSequenceRepository
    {

    }
}
