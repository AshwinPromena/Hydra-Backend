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
    public class BadgeSequenceController(IBadgeSequenceService badgeSequenceService) : ControllerBase
    {
        private readonly IBadgeSequenceService _badgeSequenceService = badgeSequenceService;

        [HttpPost("[action]")]
        public async Task<ApiResponse> AddBadgeSequence(string sequenceName)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeSequenceService.AddBadgeSequence(sequenceName);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> UpdateBadgeSequence(int sequenceId, string sequenceName)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeSequenceService.UpdateBadgeSequence(sequenceId, sequenceName);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> DeleteBadgeSequence(int sequenceId)
        {
            if (!ModelState.IsValid)
                return new ApiResponse(400, ResponseConstants.BadRequest);

            return await _badgeSequenceService.DeleteBadgeSequence(sequenceId);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<BadgeSequenceOutputModel>> GetBadgeSequenceById(int sequenceId)
        {
            if (!ModelState.IsValid)
                return new ServiceResponse<BadgeSequenceOutputModel>(400, ResponseConstants.BadRequest, null);

            return await _badgeSequenceService.GetBadgeSequenceById(sequenceId);
        }

        [HttpPost("[action]")]
        public async Task<PagedResponse<List<BadgeSequenceOutputModel>>> GetAllBadgeSequences(PagedResponseInput model)
        {
            if (!ModelState.IsValid)
                return new PagedResponse<List<BadgeSequenceOutputModel>>()
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

            return await _badgeSequenceService.GetAllBadgeSequences(model);
        }
    }
}
