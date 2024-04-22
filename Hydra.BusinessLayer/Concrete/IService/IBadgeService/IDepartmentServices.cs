using Hydra.BusinessLayer.Concrete.Service.BadgeService;
using Hydra.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.BusinessLayer.Concrete.IService.IBadgeService
{
    public interface IDepartmentServices
    {
        Task<ApiResponse> AddDepartment(string departmentName);

        Task<ApiResponse> UpdateDepartment(int departmentId, string departmentName);

        Task<ApiResponse> DeleteDepartment(int departmentId);

        Task<ServiceResponse<DepartmentOutputModel>> GetDepartmentById(long departmentId);

        Task<PagedResponse<List<DepartmentOutputModel>>> GetAllDepartments(PagedResponseInput model);
    }
}
