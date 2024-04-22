using DocumentFormat.OpenXml.EMMA;
using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.BusinessLayer.Concrete.Service.BadgeService;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.BadgeController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController(IDepartmentServices departmentServices) : ControllerBase
    {
        private readonly IDepartmentServices _departmentServices = departmentServices;

        [HttpPost("[action]")]
        public async Task<ApiResponse> AddDepartment(string departmentName)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _departmentServices.AddDepartment(departmentName);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> UpdateDepartment(int departmentId, string departmentName)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _departmentServices.UpdateDepartment(departmentId, departmentName);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> DeleteDepartment(int departmentId)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _departmentServices.DeleteDepartment(departmentId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<DepartmentOutputModel>> GetDepartmentById(long departmentId)
        {
            if (!ModelState.IsValid)
                return new ServiceResponse<DepartmentOutputModel>(400, ResponseConstants.BadRequest, null);

            return await _departmentServices.GetDepartmentById(departmentId);
        }

        [HttpPost("[action]")]
        public async Task<PagedResponse<List<DepartmentOutputModel>>> GetAllDepartments(PagedResponseInput model)
        {
            if (!ModelState.IsValid)
                return new PagedResponse<List<DepartmentOutputModel>>()
                {
                    Data = [],
                    PageIndex = model.PageIndex,
                    PageSize = model.PageSize,
                    SearchString = model.SearchString,
                    TotalRecords = 0,
                    HasNextPage = false,
                    HasPreviousPage = false,
                    StatusCode = 400,
                    Message = ResponseConstants.BadRequest
                };

            return await _departmentServices.GetAllDepartments(model);
        }

    }
}
