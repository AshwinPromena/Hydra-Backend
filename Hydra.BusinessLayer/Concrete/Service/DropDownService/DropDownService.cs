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

        public async Task<ServiceResponse<List<DepartmentDropDownModel>>> GetAllDepartment()
        {
            return new(200, ResponseConstants.Success, await _unitOfWork.DepartmentRepository
                                                                        .FindByCondition(x => x.IsActive)
                                                                        .Select(s => new DepartmentDropDownModel
                                                                        {
                                                                            DepartmentId = s.Id,
                                                                            DepartmentName = s.Name
                                                                        }).ToListAsync());
        }

        public async Task<ServiceResponse<List<AccessLevelDropDownModel>>> GetAllAccessLevel()
        {
            return new(200, ResponseConstants.Success, await _unitOfWork.AccessLevelRepository
                                                                  .FindByCondition(x => x.Id > 0)
                                                                  .Select(s => new AccessLevelDropDownModel
                                                                  {
                                                                      AccessLevelId = s.Id,
                                                                      AccessLevelName = s.Name
                                                                  }).ToListAsync());
        }

        public async Task<ServiceResponse<List<UserDropDownModel>>> GetAllApprovalUsers()
        {
            return new(200, ResponseConstants.Success, await _unitOfWork.UserRepository
                                                                        .FindByCondition(x => x.IsActive &&
                                                                       x.IsApproved &&
                                                                       x.UserRole.FirstOrDefault().RoleId == (int)Roles.Staff)
                                                                        .Select(s => new UserDropDownModel
                                                                        {
                                                                            UserId = s.Id,
                                                                            UserName = (string.IsNullOrEmpty(s.FirstName) ? null : s.FirstName) + (!string.IsNullOrEmpty(s.FirstName) && !string.IsNullOrEmpty(s.LastName) ? " " : null) + (string.IsNullOrEmpty(s.LastName) ? null : s.LastName)
                                                                        }).ToListAsync());
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

        public ServiceResponse<List<BadgeSortByDropDownModel>> GetBadgeSortOptions()
        {
            return new(200, ResponseConstants.Success, [new BadgeSortByDropDownModel { SortId = 0, SortName = BadgeSortBy.All.ToString()} ,
                                                        new BadgeSortByDropDownModel { SortId = 1, SortName = BadgeSortBy.Badge.ToString()} ,
                                                        new BadgeSortByDropDownModel { SortId = 2, SortName = BadgeSortBy.Certificate.ToString()} ,
                                                        new BadgeSortByDropDownModel { SortId = 3, SortName = BadgeSortBy.License.ToString()} ,
                                                        new BadgeSortByDropDownModel { SortId = 4, SortName = BadgeSortBy.Miscellaneous.ToString()}]);
        }

        public ServiceResponse<List<StaffSortByDropDownModel>> GetStaffSortOptions()
        {
            return new(200, ResponseConstants.Success, [new StaffSortByDropDownModel { SortId = 0, SortName = StaffSortBy.All.ToString()} ,
                                                        new StaffSortByDropDownModel { SortId = 1, SortName = StaffSortBy.Email.ToString()} ,
                                                        new StaffSortByDropDownModel { SortId = 2, SortName = StaffSortBy.UserName.ToString()}]);
        }

        public async Task<ServiceResponse<List<BadgeSequenceOutputModel>>> GetBadgeSequence()
        {
            return new(200, ResponseConstants.Success, await _unitOfWork.BadgeSequenceRepository.FindByCondition(x => x.IsActive).Select(x => new BadgeSequenceOutputModel
            {
                SequenceId = x.Id,
                SequenceName = x.Name,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            }).ToListAsync());
        }

        public ServiceResponse<List<DeletedUserDropDownModel>> GetDeletedUserSortOptions()
        {
            return new(200, ResponseConstants.Success, [new DeletedUserDropDownModel { TypeId = 1, TypeName = DeletedUserOptions.All.ToString()} ,
                                                        new DeletedUserDropDownModel { TypeId = 2, TypeName = DeletedUserOptions.Learner.ToString()} ,
                                                        new DeletedUserDropDownModel { TypeId = 3, TypeName = DeletedUserOptions.Staff.ToString()}]);
        }

        public async Task<ServiceResponse<List<BadgeTypeDropDownModel>>> GetBadgeType()
        {
            return new(200, ResponseConstants.Success, await _unitOfWork.BadgeTypeRepository.FindByCondition(x => x.Id > 0).Select(s => new BadgeTypeDropDownModel
            {
                BadgeTypeId = s.Id,
                BadgeTypeName = s.Name,
            }).ToListAsync());
        }
    }
}
