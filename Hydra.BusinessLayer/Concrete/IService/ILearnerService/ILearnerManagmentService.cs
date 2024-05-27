using Hydra.Common.Models;
namespace Hydra.BusinessLayer.Repository.IService.ILearnerService
{
    public interface ILearnerManagmentService
    {
        Task<ServiceResponse<LearnerDashBoardModel>> LearnerDashBoard();

        Task<ServiceResponse<List<ExistingLearnerModel>>> BatchUploadLeraner(List<AddLearnerModel> model);

        Task<ServiceResponse<GetS3UrlModel>> DownloadSampleExcelFile();

        Task<ApiResponse> AddLearner(AddLearnerModel model);

        Task<PagedResponse<List<GetLearnerModel>>> GetAllLearners(GetAllLearnerInputModel model);

        Task<ServiceResponse<GetLearnerByIdModel>> GetLearnerById(long userId);

        Task<ApiResponse> UpdateLearner(UpdateLearnerModel model);

        Task<ApiResponse> RemoveProfilePicture(long userId);

        Task<ApiResponse> RevokeBadgeFromLearner(RevokeBadgeModel model);

        Task<ApiResponse> RemoveBadge(RemoveBadgeModel model);

        Task<ApiResponse> RemoveLearners(List<RemoveLearnerModel> model);
    }
}
