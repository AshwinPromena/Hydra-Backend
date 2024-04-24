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
    public class BadgeSequenceRepository(HydraContext context) : CommonRepository<BadgeSequence>(context), IBadgeSequenceRepository
    {
        private readonly HydraContext _context = context;

        public async Task DeleteBadgeFields(long badgeId)
        {
            var badgeFields = await _context.BadgeField.Where(x => x.BadgeId == badgeId).ToListAsync();
            _context.RemoveRange(badgeFields);
        }
    }
}
