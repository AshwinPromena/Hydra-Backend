using Hydra.BusinessLayer.Repository.IService.ILearnerService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Hydra.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hydra.BusinessLayer.Repository.Service.LearnerService
{
    public class LearnerManagmentService(HydraContext context) : ILearnerManagmentService
    {
        private readonly HydraContext _context = context;


        public async Task<ApiResponse> AddLearner(AddLearnerModel model)
        {
            var verifyLearner = await _context.User.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (verifyLearner != null)
            {
                return new(400, ResponseConstants.LearnerExists);
            }
            var learner = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };
            await _context.AddAsync(learner);
            await _context.SaveChangesAsync();
            return new(200, ResponseConstants.LearnerAdded);
        }


        //public async Task<ServiceResponse<List<LearnerBadge>>> GetAllLearner()
        //{
        //    var learnerList = await _context.Learner.ToListAsync();
        //    return new ServiceResponse<List<LearnerBadge>>
        //    {
        //        Data = learnerList,
        //        Message = ResponseConstants.Success,
        //        StatusCode = 200,
        //    };
        //}
    }
}
