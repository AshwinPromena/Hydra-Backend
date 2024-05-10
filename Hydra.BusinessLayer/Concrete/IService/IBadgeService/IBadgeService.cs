using Hydra.Common.Models;

namespace Hydra.BusinessLayer.Concrete.IService.IBadgeService
{
    public interface IBadgeService
    {
        Task<ApiResponse> AddBadge(AddBadgeModel model);

        Task<ApiResponse> UpdateBadge(UpdateBadgeModel model);

        Task<ApiResponse> DeleteBadge(DeleteBadgeModel model);

        Task<ServiceResponse<GetBadgeModel>> GetBadgeById(long badgeId);

        Task<PagedResponse<List<GetBadgeModel>>> GetAllBadges(PagedResponseInput model);

        Task<ApiResponse> AssignBadges(AssignBadgeModel model);
    }
}
