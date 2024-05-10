using Hydra.Common.Models;

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
