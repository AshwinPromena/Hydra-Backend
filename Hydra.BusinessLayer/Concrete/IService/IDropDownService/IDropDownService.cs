using Hydra.Common.Models;
namespace Hydra.BusinessLayer.Repository.IService.IDropDownService
{
    public interface IDropDownService
    {
        Task<PagedResponse<List<DepartmentModel>>> GetAllDepartment(PagedResponseInput model);

        Task<PagedResponse<List<AccessLevelModel>>> GetAllAccessLevel(PagedResponseInput model);
    }
}
