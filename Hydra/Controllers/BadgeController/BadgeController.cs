using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.BusinessLayer.Concrete.Service.BadgeService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.BadgeController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BadgeController(IBadgeService badgeService) : ControllerBase
    {
        private readonly IBadgeService _badgeService = badgeService;

        [HttpPost("[action]")]
        public async Task<ApiResponse> AddBadge(AddBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeService.AddBadge(model);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> UpdateBadge(UpdateBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeService.UpdateBadge(model);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> DeleteBadge(DeleteBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeService.DeleteBadge(model);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<GetBadgeModel>> GetBadgeById(long badgeId)
        {
            if (!ModelState.IsValid)
                return new ServiceResponse<GetBadgeModel>(400, ResponseConstants.BadRequest, null);

            return await _badgeService.GetBadgeById(badgeId);
        }

        [HttpPost("[action]")]
        public async Task<PagedResponse<List<GetBadgeModel>>> GetAllBadges(PagedResponseInput model)
        {
            if (!ModelState.IsValid)
                return new PagedResponse<List<GetBadgeModel>>()
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

            return await _badgeService.GetAllBadges(model);
        }
    }
}
