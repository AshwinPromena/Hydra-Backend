using Hydra.Common.Models;
namespace Hydra.BusinessLayer.Repository.IService.ILearnerService
{
    public interface ILearnerManagmentService
    {
        Task<ServiceResponse<LearnerDashBoardModel>> LearnerDashBoard();

        Task<ServiceResponse<List<ExistingLearnerModel>>> BatchUploadLeraner(List<AddLearnerModel> model);

        Task<string> DownloadSampleExcelFile();

        Task<ApiResponse> AddLearner(AddLearnerModel model);

        Task<ApiResponse> AssignBadgeToLearners(AssignBadgeModel model);

        Task<PagedResponse<List<GetLearnerModel>>> GetAllLearners(GetAllLearnerInputModel model);

        Task<ServiceResponse<GetLearnerByIdModel>> GetLearnerById(long userId);

        Task<ApiResponse> UpdateLearner(UpdateLearnerModel model);

        Task<ApiResponse> RevokeBadgeFromLearner(RevokeBadgeModel model);

        Task<ApiResponse> DeleteBadge(DeleteBadgeModel model);

        Task<ApiResponse> RemoveLearners(RemoveLearnerModel model);

        Task<ApiResponse> UpdateLearner(UpdateLearnerModel model);
    }
}
