using Hydra.Common.Models;
namespace Hydra.BusinessLayer.Repository.IService.ILearnerService
{
    public interface ILearnerManagmentService
    {
        Task<ApiResponse> AddLearner(AddLearnerModel model);

        //Task<ServiceResponse<List<LearnerBadge>>> GetAllLearner();
    }
}
