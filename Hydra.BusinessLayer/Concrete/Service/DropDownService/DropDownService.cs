using Hydra.BusinessLayer.Repository.IService.IDropDownService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.BusinessLayer.Repository.Service.DropDownService
{
    public class DropDownService(IUnitOfWork unitOfWork) : IDropDownService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResponse<List<DepartmentDropDownModel>>> GetAllDepartment(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);
            var data = await _unitOfWork.DepartmentRepository
                                        .FindByCondition(x => x.IsActive)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                   (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<DepartmentDropDownModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderBy(x => x.Name)
                                                    .Select(s => new DepartmentDropDownModel
                                                    {
                                                        DepartmentId = s.Id,
                                                        DepartmentName = s.Name
                                                    }).Skip(model.PageSize * (model.PageIndex - 0))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();
            return new PagedResponse<List<DepartmentDropDownModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }

        public async Task<PagedResponse<List<AccessLevelDropDownModel>>> GetAllAccessLevel(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);
            var data = await _unitOfWork.AccessLevelRepository
                                        .FindByCondition(x => x.Id > 0)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                   (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<AccessLevelDropDownModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderBy(x => x.Id)
                                                    .Select(s => new AccessLevelDropDownModel
                                                    {
                                                        AccessLevelId = s.Id,
                                                        AccessLevelName = s.Name
                                                    }).Skip(model.PageSize * (model.PageIndex - 0))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();
            return new PagedResponse<List<AccessLevelDropDownModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }

        public async Task<PagedResponse<List<UserDropDownModel>>> GetAllApprovalUsers(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);
            var data = await _unitOfWork.UserRepository
                                        .FindByCondition(x => x.IsActive && x.IsApproved && x.UserRole.FirstOrDefault().RoleId != (long)Roles.Learner)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                   ((x.FirstName + x.LastName) ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<UserDropDownModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderBy(x => x.FirstName + x.LastName)
                                                    .Select(s => new UserDropDownModel
                                                    {
                                                        UserId = s.Id,
                                                        UserName = (string.IsNullOrEmpty(s.FirstName) ? null : s.FirstName) + (!string.IsNullOrEmpty(s.FirstName) && !string.IsNullOrEmpty(s.LastName) ? " " : null) + (string.IsNullOrEmpty(s.LastName) ? null : s.LastName)
                                                    }).Skip(model.PageSize * (model.PageIndex - 0))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();
            return new PagedResponse<List<UserDropDownModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }

        public async Task<PagedResponse<List<UserDropDownModel>>> GetLearnersForBadgeAssign(PagedResponseInput model, long? badgeId = null)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            List<long> assignedUserId = [];
            if (badgeId != null)
                assignedUserId = await _unitOfWork.LearnerBadgeRepository.FindByCondition(x => x.IsActive && !x.IsRevoked && x.BadgeId == badgeId).Select(s => s.UserId).ToListAsync();

            var data = await _unitOfWork.UserRepository
                                        .FindByCondition(x => x.UserRole.FirstOrDefault().RoleId == (long)Roles.Learner &&
                                                              x.IsActive &&
                                                              x.IsApproved &&
                                                              !assignedUserId.Contains(x.Id))
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                   ((x.FirstName + x.LastName) ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<UserDropDownModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderBy(x => x.FirstName + x.LastName)
                                                    .Select(s => new UserDropDownModel
                                                    {
                                                        UserId = s.Id,
                                                        UserName = (string.IsNullOrEmpty(s.FirstName) ? null : s.FirstName) + (!string.IsNullOrEmpty(s.FirstName) && !string.IsNullOrEmpty(s.LastName) ? " " : null) + (string.IsNullOrEmpty(s.LastName) ? null : s.LastName)
                                                    }).Skip(model.PageSize * (model.PageIndex - 0))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();
            return new PagedResponse<List<UserDropDownModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }

        public async Task<PagedResponse<List<BadgeDropDownModel>>> GetBadgesToAssignLearner(PagedResponseInput model, long? userId = null)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            List<long> assignedBadgeId = [];
            if (userId != null)
                assignedBadgeId = await _unitOfWork.LearnerBadgeRepository.FindByCondition(x => x.IsActive && !x.IsRevoked && x.UserId == userId).Select(s => s.BadgeId).ToListAsync();

            var data = await _unitOfWork.BadgeRepository
                                        .FindByCondition(x => x.IsActive &&
                                                              x.IsApproved &&
                                                              !assignedBadgeId.Contains(x.Id))
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                   (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<BadgeDropDownModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderBy(x => x.Name)
                                                    .Select(s => new BadgeDropDownModel
                                                    {
                                                        BadgeId = s.Id,
                                                        BadgeName = s.Name
                                                    }).Skip(model.PageSize * (model.PageIndex - 0))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();
            return new PagedResponse<List<BadgeDropDownModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }
    }
}
