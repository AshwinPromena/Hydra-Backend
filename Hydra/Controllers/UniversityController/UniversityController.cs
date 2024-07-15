using Hydra.BusinessLayer.Concrete.IService.IUniversityService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.UniversityController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController(IUniversityService universityService) : ControllerBase
    {
        private readonly IUniversityService _universityService = universityService;


        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        public async Task<ApiResponse> AddUniversity(AddUniversityModel model) 
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _universityService.AddUniversity(model);
        }


        [HttpGet("[action]"), Authorize(Roles = "Admin , Staff")]
        public async Task<ServiceResponse<List<GetUniversityByIdModel>>> GetUniversityById(long universityId)
        {
            if(universityId == 0)
                return new(400, ResponseConstants.InvalidId);

            return await _universityService.GetUniversityById(universityId);
        }


        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        public async Task<PagedResponse<List<GetAllUniversityModel>>> GetAllUniversity(GetAllUniversityModel model)
        {
            return await _universityService.GetAllUniversity(model);
        }


        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        public async Task<ApiResponse> UpdateUniversity(UpdateUniversityModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _universityService.UpdateUniversity(model);
        }


        [HttpDelete("[action]"), Authorize(Roles = "Admin , Staff")]
        public async Task<ApiResponse> DeleteUniversity(long universityId)
        {
            if (universityId == 0)
                return new(400, ResponseConstants.InvalidId);

            return await _universityService.DeleteUniversity(universityId);
        }
    }
}
