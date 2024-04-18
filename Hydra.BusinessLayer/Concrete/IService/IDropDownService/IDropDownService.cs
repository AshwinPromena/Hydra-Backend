using Hydra.Common.Models;
namespace Hydra.BusinessLayer.Repository.IService.IDropDownService
{
    public interface IDropDownService
    {
        Task<ServiceResponse<List<DepartmentModel>>> GetAllDepartment();

        Task<ServiceResponse<List<AccessLevelModel>>> GetAllAccessLevel();
    }
}
