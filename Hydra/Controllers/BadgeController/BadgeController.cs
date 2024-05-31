using Hydra.BusinessLayer.ActionFilters;
using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.BadgeController
{
    [Route("api/[controller]")]
    [ApiController]

    public class BadgeController(IBadgeService badgeService) : ControllerBase
    {
        private readonly IBadgeService _badgeService = badgeService;
        private static List<int> ParseRole = [];

        [HttpGet("[action]"), Authorize(Roles = "Admin , Staff")]
        [CustomAuthorizationFilterAttributeFilterFactory([(int)AccessLevelType.ViewEditAndDelete], [(int)Roles.Admin, (int)Roles.Staff])]
        public async Task<ServiceResponse<BadgeFactoryDashBoardModel>> BadgeFactoryDashBoard()
        {
            return await _badgeService.BadgeFactoryDashBoard();
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        [CustomAuthorizationFilterAttributeFilterFactory([(int)AccessLevelType.ViewEditAndDelete], [(int)Roles.Admin, (int)Roles.Staff])]
        public async Task<ApiResponse> AddBadge(AddBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeService.AddBadge(model);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        [CustomAuthorizationFilterAttributeFilterFactory([(int)AccessLevelType.ViewEditAndDelete, (int)AccessLevelType.ViewAndEdit], [(int)Roles.Admin, (int)Roles.Staff])]
        public async Task<ApiResponse> UpdateBadge(UpdateBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeService.UpdateBadge(model);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        [CustomAuthorizationFilterAttributeFilterFactory([(int)AccessLevelType.ViewEditAndDelete, (int)AccessLevelType.ViewAndEdit], [(int)Roles.Admin, (int)Roles.Staff])]
        public async Task<ApiResponse> DeleteBadge(DeleteBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeService.DeleteBadge(model);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        [CustomAuthorizationFilterAttributeFilterFactory([(int)AccessLevelType.ViewOnly, (int)AccessLevelType.ViewEditAndDelete], [(int)Roles.Admin, (int)Roles.Staff])]
        public async Task<ServiceResponse<GetBadgeModel>> GetBadgeById(long badgeId)
        {
            if (!ModelState.IsValid)
                return new ServiceResponse<GetBadgeModel>(400, ResponseConstants.BadRequest, null);

            return await _badgeService.GetBadgeById(badgeId);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        [CustomAuthorizationFilterAttributeFilterFactory([(int)AccessLevelType.ViewOnly, (int)AccessLevelType.ViewEditAndDelete], [(int)Roles.Staff, (int)Roles.Admin])]
        public async Task<PagedResponse<List<GetBadgeModel>>> GetAllBadges(GetAllBadgeInputModel model)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _badgeService.GetAllBadges(model);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        [CustomAuthorizationFilterAttributeFilterFactory([(int)AccessLevelType.ViewEditAndDelete], [(int)Roles.Admin, (int)Roles.Staff])]
        public async Task<ServiceResponse<List<NotApprovedBadgeModel>>> AssignBadges(AssignBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _badgeService.AssignBadges(model);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff")]
        [CustomAuthorizationFilterAttributeFilterFactory([(int)AccessLevelType.ViewEditAndDelete], [(int)Roles.Admin, (int)Roles.Staff])]
        public async Task<PagedResponse<List<GetBadgeModel>>> GetUnApprovedBadges(GetUnApprovedBadgeInputModel model)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _badgeService.GetUnApprovedBadges(model);
        }
    }
}
