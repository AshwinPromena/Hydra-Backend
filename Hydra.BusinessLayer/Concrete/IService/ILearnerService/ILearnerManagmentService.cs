using Hydra.Common.Models;
using Microsoft.AspNetCore.Http;
namespace Hydra.BusinessLayer.Repository.IService.ILearnerService
{
    public interface ILearnerManagmentService
    {
        Task<ServiceResponse<LearnerDashBoardModel>> LearnerDashBoard();

        Task<ServiceResponse<List<ExistingLearnerModel>>> BatchUploadLeraner(IFormFile file);

        Task<string> DownloadSampleExcelFile();

        Task<ApiResponse> AddLearner(AddLearnerModel model);

        Task<ApiResponse> AssignBadgeToLearners(AssignBadgeModel model);

        //Task<ServiceResponse<List<LearnerBadge>>> GetAllLearner();
    }
}
