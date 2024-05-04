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
        public async Task<PagedResponse<List<DepartmentDropDownModel>>> GetAllDepartment(PagedResponseInput model)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _dropDownService.GetAllDepartment(model);
        }

        [HttpPost("[action]")]
        public async Task<PagedResponse<List<AccessLevelDropDownModel>>> GetAllAccessLevel(PagedResponseInput model)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _dropDownService.GetAllAccessLevel(model);
        }

        [HttpPost("[action]"), Authorize]
        public async Task<PagedResponse<List<UserDropDownModel>>> GetAllApprovalUsers(PagedResponseInput model)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _dropDownService.GetAllApprovalUsers(model);
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
