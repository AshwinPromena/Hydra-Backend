using Hydra.Common.Models;
namespace Hydra.BusinessLayer.Repository.IService.IDropDownService
{
    public interface IDropDownService
    {
        Task<ServiceResponse<List<DepartmentDropDownModel>>> GetAllDepartment();

        Task<ServiceResponse<List<AccessLevelDropDownModel>>> GetAllAccessLevel();

        Task<ServiceResponse<List<UserDropDownModel>>> GetAllApprovalUsers(long departmentId);

        Task<PagedResponse<List<UserDropDownModel>>> GetLearnersForBadgeAssign(PagedResponseInput model, long? badgeId = null);

        Task<PagedResponse<List<BadgeDropDownModel>>> GetBadgesToAssignLearner(PagedResponseInput model, long? userId = null);

        ServiceResponse<List<BadgeSortByDropDownModel>> GetBadgeSortOptions();

        ServiceResponse<List<StaffSortByDropDownModel>> GetStaffSortOptions();

        Task<ServiceResponse<List<BadgeSequenceOutputModel>>> GetBadgeSequence();

        ServiceResponse<List<DeletedUserDropDownModel>> GetDeletedUserSortOptions();

        Task<ServiceResponse<List<BadgeTypeDropDownModel>>> GetBadgeType();
    }
}
