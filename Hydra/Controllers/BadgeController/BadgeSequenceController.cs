using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.BadgeController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadgeSequenceController(IBadgeSequenceService badgeSequenceService) : ControllerBase
    {
        private readonly IBadgeSequenceService _badgeSequenceService = badgeSequenceService;

        [HttpPost("[action]"), Authorize(Roles = "Admin, Staff")]
        public async Task<ApiResponse> AddBadgeSequence(string sequenceName)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeSequenceService.AddBadgeSequence(sequenceName);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin, Staff")]
        public async Task<ApiResponse> UpdateBadgeSequence(int sequenceId, string sequenceName)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeSequenceService.UpdateBadgeSequence(sequenceId, sequenceName);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin, Staff")]
        public async Task<ApiResponse> DeleteBadgeSequence(int sequenceId)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeSequenceService.DeleteBadgeSequence(sequenceId);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin, Staff")]
        public async Task<ServiceResponse<BadgeSequenceOutputModel>> GetBadgeSequenceById(int sequenceId)
        {
            if (!ModelState.IsValid)
                return new ServiceResponse<BadgeSequenceOutputModel>(400, ResponseConstants.BadRequest, null);

            return await _badgeSequenceService.GetBadgeSequenceById(sequenceId);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin, Staff")]
        public async Task<PagedResponse<List<BadgeSequenceOutputModel>>> GetAllBadgeSequences(PagedResponseInput model)
        {
            if (!ModelState.IsValid)
                return new() { StatusCode = 400, Message = ResponseConstants.BadRequest };

            return await _badgeSequenceService.GetAllBadgeSequences(model);
        }
    }
}
