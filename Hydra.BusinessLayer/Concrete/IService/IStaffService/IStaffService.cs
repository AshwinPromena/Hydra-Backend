using Hydra.Common.Models;

namespace Hydra.BusinessLayer.Concrete.IService.IStaffService
{
    public interface IStaffService
    {
        Task<ApiResponse> AddStaff(AddStaffModel model);

        Task<ApiResponse> UpdateStaff(UpdateStaffModel model);

        Task<ApiResponse> DeleteStaff(List<DeleteStaffModel> model);

        Task<ApiResponse> ArchivedStaffs(ArchiveStaffModel model);

        Task<ServiceResponse<GetStaffModel>> GetStaffById(long userId);

        Task<PagedResponse<List<GetStaffModel>>> GetAllStaff(GetAllStaffInputModel model);

        Task<ApiResponse> ApproveStaffUser(long staffUserId);

        Task<ApiResponse> ApproveBadge(ApproveBadgeModel model);
    }
}
