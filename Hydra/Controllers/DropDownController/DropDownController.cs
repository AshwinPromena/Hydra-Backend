using Hydra.BusinessLayer.Repository.IService.IDropDownService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.DropDownController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownController(IDropDownService dropDownService) : ControllerBase
    {
        private readonly IDropDownService _dropDownService = dropDownService;

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<DepartmentDropDownModel>>> GetAllDepartment()
        {
            return await _dropDownService.GetAllDepartment();
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<AccessLevelDropDownModel>>> GetAllAccessLevel()
        {
            return await _dropDownService.GetAllAccessLevel();
        }

        [HttpPost("[action]"), Authorize]
        public async Task<ServiceResponse<List<UserDropDownModel>>> GetAllApprovalUsers()
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _dropDownService.GetAllApprovalUsers();
        }

        [HttpPost("[action]"), Authorize]
        public async Task<PagedResponse<List<UserDropDownModel>>> GetLearnersForBadgeAssign(PagedResponseInput model, long? badgeId = null)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _dropDownService.GetLearnersForBadgeAssign(model, badgeId);
        }

        [HttpPost("[action]"), Authorize]
        public async Task<PagedResponse<List<BadgeDropDownModel>>> GetBadgesToAssignLearner(PagedResponseInput model, long? userId = null)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _dropDownService.GetBadgesToAssignLearner(model, userId);
        }
    }
}
