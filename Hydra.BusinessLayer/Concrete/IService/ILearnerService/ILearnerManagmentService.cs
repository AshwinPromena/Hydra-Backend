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

        Task<PagedResponse<List<GetLearnerModel>>> GetAllLearners(PagedResponseInput model);

        Task<ServiceResponse<GetLearnerModel>> GetLearnerById(long userId);

        Task<PagedResponse<List<GetLearnerModel>>> GetRecentlyAddedLearner(DateTime fromDate, DateTime toDate,PagedResponseInput model);

        Task<ApiResponse> RevokeBadgeFromLearner(RevokeBadgeModel model);

        Task<ApiResponse> RemoveLearners(RemoveLearnerModel model);
    }
}
