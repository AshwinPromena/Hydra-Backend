using Hydra.Common.Models;

namespace Hydra.BusinessLayer.Concrete.IService.IUniversityService
{
    public interface IUniversityService
    {
        Task<ApiResponse> AddUniversity(AddUniversityModel model);

        Task<ServiceResponse<List<GetUniversityByIdModel>>> GetUniversityById(long universityId);

        Task<PagedResponse<List<GetAllUniversityModel>>> GetAllUniversity(GetAllUniversityModel model);

        Task<ApiResponse> UpdateUniversity(UpdateUniversityModel model);

        Task<ApiResponse> DeleteUniversity(long universityId);
    }
}
