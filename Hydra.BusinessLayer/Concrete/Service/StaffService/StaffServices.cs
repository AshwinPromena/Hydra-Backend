using Hydra.BusinessLayer.Concrete.IService.IStaffService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hydra.BusinessLayer.Concrete.Service.StaffService
{
    public class StaffServices(IUnitOfWork unitOfWork, IStorageService storageService, ICurrentUserService currentUserService) : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IStorageService _storageService = storageService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ApiResponse> AddStaff(AddStaffModel model)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower()) && x.IsActive).FirstOrDefaultAsync();
            if (user != null)
                return new(400, ResponseConstants.UserNameExists);

            user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MobileNumber = model.MobileNumber,
                IsActive = true,
                IsApproved = false,
                IsArchived = false,
                AccessLevelId = model.AccessLevelId,
                DepartmentId = model.DepartmentId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
            user.UserRole.Add(new()
            {
                RoleId = (long)Roles.Staff,
            });

            if (!string.IsNullOrEmpty(model.ProfilePicture))
                user.ProfilePicture = _storageService.UploadFile(ResponseConstants.Mediapath, model.ProfilePicture).Result.Data;

            await _unitOfWork.UserRepository.Create(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.Success);
        }

        public async Task<ApiResponse> UpdateStaff(UpdateStaffModel model)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.Id == model.UserId && x.IsActive).FirstOrDefaultAsync();
            if (user == null)
                return new ApiResponse(404, ResponseConstants.InvalidUserId);

            var existingUser = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Replace(" ", string.Empty) == model.UserName.ToLower().Replace(" ", string.Empty) && x.Id != model.UserId).FirstOrDefaultAsync();
            if (existingUser != null)
                return new ApiResponse(400, ResponseConstants.UserNameExists);

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.MobileNumber = model.MobileNumber;
            user.AccessLevelId = model.AccessLevelId;
            user.DepartmentId = model.DepartmentId;
            user.UpdatedDate = DateTime.UtcNow;

            user.ProfilePicture = !string.IsNullOrEmpty(model.ProfilePicture)
                                           ? (await _storageService.UploadFile(FileExtentionService.GetMediapath(), model.ProfilePicture)).Data
                                           : user.ProfilePicture;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new ApiResponse(200, ResponseConstants.StaffUpdated);
        }

        public async Task<ApiResponse> DeleteStaff(List<DeleteStaffModel> model)
        {
            var userList = await _unitOfWork.UserRepository
                                            .FindByCondition(x => model.Select(s => s.UserIds).Contains(x.Id) &&
                                           x.UserRole.FirstOrDefault().RoleId == (int)Roles.Learner)
                                            .Include(i => i.DeletedUser).AsNoTracking()
                                            .ToListAsync();

            if (userList.IsNullOrEmpty())
                return new(400, ResponseConstants.InvalidUserId);

            foreach (var staff in userList)
            {
                var currentDate = DateTime.UtcNow;
                staff.IsActive = false;
                staff.UpdatedDate = currentDate;

                var reasonModel = model.FirstOrDefault(m => m.UserIds == staff.Id);
                if (reasonModel is not null)
                {
                    staff.DeletedUser.Add(new DeletedUser
                    {
                        UserId = _currentUserService.UserId,
                        Name = _currentUserService.Name,
                        Email = _currentUserService.Email,
                        DeletedUserId = staff.Id,
                        Reason = reasonModel.Reason,
                        DeleteDate = currentDate
                    });
                }
            }

            _unitOfWork.UserRepository.UpdateRange(userList);
            await _unitOfWork.UserRepository.CommitChanges();
            return new ApiResponse(200, ResponseConstants.StaffDeleted);
        }

        public async Task<ApiResponse> ArchivedStaffs(ArchiveStaffModel model)
        {
            var userList = await _unitOfWork.UserRepository.FindByCondition(x => x.IsActive && model.UserIds.Contains(x.Id)).ToListAsync();

            foreach (var staff in userList)
            {
                staff.IsArchived = !staff.IsArchived;
                staff.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.UserRepository.Update(staff);
            }

            await _unitOfWork.UserRepository.CommitChanges();
            return new ApiResponse(200, ResponseConstants.StaffUpdated);
        }

        public async Task<ServiceResponse<GetStaffModel>> GetStaffById(long userId)
        {
            var staff = await _unitOfWork.UserRepository.FindByCondition(x => x.Id == userId && x.IsActive)
                                                        .Select(s => new GetStaffModel()
                                                        {
                                                            UserId = s.Id,
                                                            UserName = s.UserName,
                                                            FirstName = s.FirstName,
                                                            LastName = s.LastName,
                                                            Email = s.Email,
                                                            IsArchived = s.IsArchived,
                                                            IsApproved = s.IsApproved,
                                                            MobileNumber = s.MobileNumber,
                                                            AccessLevelId = s.AccessLevelId,
                                                            AccessLevelName = s.AccessLevel.Name,
                                                            DepartmentId = s.DepartmentId,
                                                            DepartmentName = s.Department.Name,
                                                            ProfilePicture = s.ProfilePicture,
                                                            CreatedDate = s.CreatedDate,
                                                            UpdatedDate = s.UpdatedDate
                                                        }).FirstOrDefaultAsync();
            if (staff is null)
                return new ServiceResponse<GetStaffModel>(404, ResponseConstants.InvalidUserId);

            return new ServiceResponse<GetStaffModel>(200, ResponseConstants.Success, staff);
        }

        public async Task<ApiResponse> ApproveStaffUser(long staffUserId)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.Id == staffUserId).FirstOrDefaultAsync();
            if (user is null)
                return new(400, ResponseConstants.InvalidUserId);
            user.IsApproved = true;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();
            return new(200, ResponseConstants.Success);
        }

        public async Task<PagedResponse<List<GetStaffModel>>> GetAllStaff(PagedResponseInput model, bool IsArchived = false)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var data = await _unitOfWork.UserRepository
                                        .FindByCondition(x => x.IsActive && x.IsArchived == IsArchived && x.UserRole.FirstOrDefault().RoleId == (long)Roles.Staff)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                    ((x.FirstName + x.LastName) ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<GetStaffModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderByDescending(x => x.UpdatedDate).Select(s => new GetStaffModel
                                            {
                                                UserId = s.Id,
                                                UserName = s.UserName,
                                                FirstName = s.FirstName,
                                                LastName = s.LastName,
                                                Email = s.Email,
                                                IsArchived = s.IsArchived,
                                                IsApproved = s.IsApproved,
                                                MobileNumber = s.MobileNumber,
                                                AccessLevelId = s.AccessLevelId,
                                                AccessLevelName = s.AccessLevel.Name,
                                                DepartmentId = s.DepartmentId,
                                                DepartmentName = s.Department.Name,
                                                ProfilePicture = s.ProfilePicture,
                                                CreatedDate = s.CreatedDate,
                                                UpdatedDate = s.UpdatedDate
                                            })
                                                    .Skip(model.PageSize * (model.PageIndex))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();

            return new PagedResponse<List<GetStaffModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200,
            };
        }

        public async Task<ApiResponse> ApproveBadge(ApproveBadgeModel model)
        {
            var currentUser = _currentUserService.UserId;
            var badge = await _unitOfWork.BadgeRepository
                                         .FindByCondition(x => model.BadgeIds.Contains(x.Id) &&
                                        x.IsActive && x.IsApproved == false &&
                                        x.ApprovalUserId == currentUser).ToListAsync();

            if (badge == null)
                return new(204, ResponseConstants.BadgesApproved);

            badge.ForEach(x =>
            {
                x.IsApproved = true;
                x.UpdatedDate = DateTime.UtcNow;
            });

            _unitOfWork.BadgeRepository.UpdateRange(badge);
            await _unitOfWork.BadgeRepository.CommitChanges();

            return new(200, ResponseConstants.ApprovedBadges.Replace("{badgeCount}", badge.Count.ToString()));
        }
    }
}
