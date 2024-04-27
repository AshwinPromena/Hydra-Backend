using Hydra.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.BusinessLayer.Concrete.IService.IStaffService
{
    public interface IStaffService
    {
        Task<ApiResponse> AddStaff(AddStaffModel model);

        Task<ApiResponse> UpdateStaff(UpdateStaffModel model);

        Task<ApiResponse> DeleteStaff(DeleteStaffModel model);

        Task<ApiResponse> ArchivedStaffs(DeleteStaffModel model);

        Task<ServiceResponse<GetStaffModel>> GetStaffById(long userId);

        Task<PagedResponse<List<GetStaffModel>>> GetAllStaff(PagedResponseInput model, bool IsArchived = false);

        Task<ApiResponse> ApproveStaffUser(long staffUserId);
    }
}
