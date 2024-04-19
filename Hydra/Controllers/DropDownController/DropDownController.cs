using Hydra.BusinessLayer.Repository.IService.IDropDownService;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.DropDownController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownController(IDropDownService dropDownService) : ControllerBase
    {
        private readonly IDropDownService _dropDownService = dropDownService;

        [HttpPost("[action]")]
        public async Task<PagedResponse<List<DepartmentModel>>> GetAllDepartment(PagedResponseInput model)
        {
            return await _dropDownService.GetAllDepartment(model);
        }

        [HttpPost("[action]")]
        public async Task<PagedResponse<List<AccessLevelModel>>> GetAllAccessLevel(PagedResponseInput model)
        {
            return await _dropDownService.GetAllAccessLevel(model);
        }

    }
}
