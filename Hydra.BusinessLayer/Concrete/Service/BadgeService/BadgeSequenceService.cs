using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.BusinessLayer.Concrete.Service.BadgeService
{
    public class BadgeSequenceService(IUnitOfWork unitOfWork) : IBadgeSequenceService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ApiResponse> AddBadgeSequence(string sequenceName)
        {
            var badgeSequence = await _unitOfWork.BadgeSequenceRepository.FindByCondition(x => x.Name.ToLower().Replace(" ", string.Empty).Equals(sequenceName.ToLower().Replace(" ", string.Empty)) && x.IsActive).FirstOrDefaultAsync();
            if (badgeSequence != null)
                return new ApiResponse(400, ResponseConstants.BadgeSequenceExists);

            badgeSequence = new BadgeSequence()
            {
                Name = sequenceName
            };

            await _unitOfWork.BadgeSequenceRepository.Create(badgeSequence);
            await _unitOfWork.BadgeSequenceRepository.CommitChanges();

            return new ApiResponse(200, ResponseConstants.BadgeSequenceAdded);
        }

        public async Task<ApiResponse> UpdateBadgeSequence(long sequenceId, string sequenceName)
        {
            var badgeSequence = await _unitOfWork.BadgeSequenceRepository.FindByCondition(x => x.Id == sequenceId && x.IsActive).FirstOrDefaultAsync();
            if (badgeSequence is null)
                return new ApiResponse(404, ResponseConstants.InvalidBadgeSequenceId);

            var existingSequence = await _unitOfWork.BadgeSequenceRepository
                                                    .FindByCondition(x => x.Name.ToLower().Replace(" ", string.Empty) == sequenceName.ToLower().Replace(" ", string.Empty) &&
                                                                          x.IsActive &&
                                                                          x.Id != sequenceId)
                                                    .FirstOrDefaultAsync();

            if (existingSequence is not null)
                return new ApiResponse(400, ResponseConstants.BadgeSequenceExists);

            badgeSequence.Name = sequenceName;
            badgeSequence.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.BadgeSequenceRepository.Update(badgeSequence);
            await _unitOfWork.BadgeSequenceRepository.CommitChanges();

            return new ApiResponse(200, ResponseConstants.BadgeSequenceUpdated);
        }

        public async Task<ApiResponse> DeleteBadgeSequence(int sequenceId)
        {
            var badgeSequence = await _unitOfWork.BadgeSequenceRepository.FindByCondition(x => x.Id == sequenceId && x.IsActive).FirstOrDefaultAsync();
            if (badgeSequence is null)
                return new ApiResponse(404, ResponseConstants.InvalidBadgeSequenceId);

            badgeSequence.IsActive = false;
            badgeSequence.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.BadgeSequenceRepository.Update(badgeSequence);
            await _unitOfWork.BadgeSequenceRepository.CommitChanges();

            return new ApiResponse(200, ResponseConstants.BadgeSequenceDeleted);
        }

        public async Task<ServiceResponse<BadgeSequenceOutputModel>> GetBadgeSequenceById(int sequenceId)
        {
            var badgeSequence = await _unitOfWork.BadgeSequenceRepository
                                                 .FindByCondition(x => x.Id == sequenceId && x.IsActive)
                                                 .Select(x => new BadgeSequenceOutputModel()
                                                 {
                                                     SequenceId = x.Id,
                                                     SequenceName = x.Name,
                                                     CreatedDate = x.CreatedDate,
                                                     UpdatedDate = x.UpdatedDate
                                                 }).FirstOrDefaultAsync();

            if (badgeSequence is null)
                return new(404, ResponseConstants.InvalidBadgeSequenceId);

            return new(200, ResponseConstants.Success, badgeSequence);
        }

        public async Task<PagedResponse<List<BadgeSequenceOutputModel>>> GetAllBadgeSequences(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var data = await _unitOfWork.BadgeSequenceRepository
                                        .FindByCondition(x => x.Id > 0)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                   (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<BadgeSequenceOutputModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderByDescending(x => x.UpdatedDate)
                                                    .Select(s => new BadgeSequenceOutputModel
                                                    {
                                                        SequenceId = s.Id,
                                                        SequenceName = s.Name,
                                                        CreatedDate = s.CreatedDate,
                                                        UpdatedDate = s.UpdatedDate
                                                    }).Skip(model.PageSize * (model.PageIndex - 1))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();

            return new PagedResponse<List<BadgeSequenceOutputModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };
        }
    }
}
