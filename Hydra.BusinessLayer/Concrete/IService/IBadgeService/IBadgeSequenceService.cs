﻿using Hydra.Common.Models;

namespace Hydra.BusinessLayer.Concrete.IService.IBadgeService
{
    public interface IBadgeSequenceService
    {
        Task<ApiResponse> AddBadgeSequence(string sequenceName);

        Task<ApiResponse> UpdateBadgeSequence(long sequenceId, string sequenceName);

        Task<ApiResponse> DeleteBadgeSequence(int sequenceId);

        Task<ServiceResponse<BadgeSequenceOutputModel>> GetBadgeSequenceById(int sequenceId);

        Task<PagedResponse<List<BadgeSequenceOutputModel>>> GetAllBadgeSequences(PagedResponseInput model);
    }
}
