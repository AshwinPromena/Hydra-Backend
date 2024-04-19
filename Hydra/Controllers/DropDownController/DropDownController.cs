using Hydra.BusinessLayer.Repository.IService.IDropDownService;
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
        public async Task<> tet()
        {
            return await _dropDownService.GetAllDepartment();
        }
    }
}
