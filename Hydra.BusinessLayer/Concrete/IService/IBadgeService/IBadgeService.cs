using Hydra.Common.Models;

namespace Hydra.BusinessLayer.Concrete.IService.IBadgeService
{
    public interface IBadgeService
    {
        Task<ServiceResponse<BadgeFactoryDashBoardModel>> BadgeFactoryDashBoard();

        Task<ApiResponse> AddBadge(AddBadgeModel model);

        Task<ApiResponse> UpdateBadge(UpdateBadgeModel model);

        Task<ApiResponse> DeleteBadge(DeleteBadgeModel model);

        Task<ServiceResponse<GetBadgeModel>> GetBadgeById(long badgeId);

        Task<PagedResponse<List<GetBadgeModel>>> GetAllBadges(GetAllBadgeInputModel model);

        Task<ServiceResponse<List<NotApprovedBadgeModel>>> AssignBadges(AssignBadgeModel model);

        Task<PagedResponse<List<GetBadgeModel>>> GetUnApprovedBadges(GetUnApprovedBadgeInputModel model);
    }
}
