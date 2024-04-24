using Hydra.Common.Models;
namespace Hydra.BusinessLayer.Repository.IService.IDropDownService
{
    public interface IDropDownService
    {
        Task<PagedResponse<List<DepartmentDropDownModel>>> GetAllDepartment(PagedResponseInput model);

        Task<PagedResponse<List<AccessLevelDropDownModel>>> GetAllAccessLevel(PagedResponseInput model);

        Task<PagedResponse<List<UserDropDownModel>>> GetAllApprovalUsers(PagedResponseInput model);

        Task<PagedResponse<List<UserDropDownModel>>> GetLearnersForBadgeAssign(PagedResponseInput model, long? badgeId = null);

        Task<PagedResponse<List<BadgeDropDownModel>>> GetBadgesToAssignLearner(PagedResponseInput model, long? userId = null);
    }
}
