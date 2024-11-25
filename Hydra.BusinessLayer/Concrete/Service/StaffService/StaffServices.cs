using Hydra.BusinessLayer.Concrete.IService.IStaffService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace Hydra.BusinessLayer.Concrete.Service.StaffService
{
    public class StaffServices(IUnitOfWork unitOfWork, IStorageService storageService, ICurrentUserService currentUserService, IEmailService emailService) : EncryptionService, IStaffService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IStorageService _storageService = storageService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IEmailService _emailService = emailService;
        private static Random random = new Random();

        public async Task<ApiResponse> AddStaff(AddStaffModel model)
        {
            var universityId = _currentUserService.UniversityId;
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower()) && x.IsActive && x.UniversityId == model.UniversityId).FirstOrDefaultAsync();
            if (user != null)
                return new(400, ResponseConstants.UserNameExists);

            var password = GeneratePassword();

            user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MobileNumber = model.MobileNumber,
                IsActive = true,
                IsApproved = true,
                IsArchived = false,
                AccessLevelId = model.AccessLevelId,
                DepartmentId = model.DepartmentId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Password = Encipher(password),
                UniversityId = universityId,
            };
            user.UserRole.Add(new()
            {
                RoleId = (long)Roles.Staff,
            });

            if (!string.IsNullOrEmpty(model.ProfilePicture))
                user.ProfilePicture = _storageService.UploadFile(ResponseConstants.Mediapath, model.ProfilePicture).Result.Data;

            await _unitOfWork.UserRepository.Create(user);
            await _unitOfWork.UserRepository.CommitChanges();
            await _emailService.SendStaffLoginCredential(model.Email, $"{model.FirstName} {model.LastName}", model.UserName, password);

            return new(200, ResponseConstants.Success);
        }

        public static string GeneratePassword()
        {
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string digitChars = "0123456789";
            const string specialChars = "!@#$%^&*()";
            const string allChars = upperChars + lowerChars + digitChars + specialChars;

            StringBuilder sb = new StringBuilder();

            sb.Append(upperChars[random.Next(upperChars.Length)]);
            sb.Append(lowerChars[random.Next(lowerChars.Length)]);
            sb.Append(digitChars[random.Next(digitChars.Length)]);
            sb.Append(specialChars[random.Next(specialChars.Length)]);

            for (int i = 4; i < 8; i++)
            {
                sb.Append(allChars[random.Next(allChars.Length)]);
            }
            return Shuffle(sb.ToString());
        }

        private static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return new string(array);
        }

        public async Task<ApiResponse> UpdateStaff(UpdateStaffModel model)
        {
            var universityId = _currentUserService.UniversityId;
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.Id == model.UserId && x.IsActive && x.UniversityId == model.UniversityId).FirstOrDefaultAsync();
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
            user.IsApproved = model.IsApproved;
            user.UniversityId = universityId;

            user.ProfilePicture = !string.IsNullOrEmpty(model.ProfilePicture)
                                           ? (await _storageService.UploadFile(FileExtentionService.GetMediapath(), model.ProfilePicture)).Data
                                           : user.ProfilePicture;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new ApiResponse(200, ResponseConstants.StaffUpdated);
        }

        public async Task<ApiResponse> DeleteStaff(List<DeleteStaffModel> model)
        {
            var universityId = _currentUserService.UniversityId;
            var userList = await _unitOfWork.UserRepository
                                            .FindByCondition(x => model.Select(s => s.UserIds).Contains(x.Id) &&
                                           x.UserRole.FirstOrDefault().RoleId == (int)Roles.Staff &&
                                           x.UniversityId == universityId)
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
                        DeletedDate = currentDate,
                        DeletedUserName = $"{staff.FirstName} {staff.LastName}",
                        DeletedUserEmail = staff.Email,
                        DeletedUserUniversityId = staff.UniversityId,
                    });
                }
            }

            _unitOfWork.UserRepository.UpdateRange(userList);
            await _unitOfWork.UserRepository.CommitChanges();
            return new ApiResponse(200, ResponseConstants.StaffDeleted);
        }

        public async Task<ApiResponse> ArchivedStaffs(ArchiveStaffModel model)
        {
            var universityId = _currentUserService.UniversityId;
            var userList = await _unitOfWork.UserRepository.FindByCondition(x => x.IsActive && model.UserIds.Contains(x.Id) && x.UniversityId == universityId).ToListAsync();

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
            var universityId = _currentUserService.UniversityId;
            var staff = await _unitOfWork.UserRepository.FindByCondition(x => x.Id == userId && x.IsActive && x.UniversityId == universityId)
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
            var universityId = _currentUserService.UniversityId;
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.Id == staffUserId && x.UserRole.FirstOrDefault().Role.Id == (long)Roles.Admin && x.UniversityId == universityId).FirstOrDefaultAsync();
            if (user is null)
                return new(400, ResponseConstants.InvalidUserId);
            user.IsApproved = true;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();
            return new(200, ResponseConstants.Success);
        }

        public async Task<PagedResponse<List<GetStaffModel>>> GetAllStaff(GetAllStaffInputModel model)
        {
            var universityId = _currentUserService.UniversityId;
            model.SearchString = model.SearchString?.ToLower().Replace(" ", string.Empty) ?? string.Empty;

            var staffQuery = _unitOfWork.UserRepository
                .FindByCondition(x => x.IsActive && x.UserRole.FirstOrDefault().RoleId == (long)Roles.Staff && x.UniversityId == universityId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.SearchString))
            {
                staffQuery = staffQuery.Where(x => (x.FirstName + x.LastName ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                   (x.UserName ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                   (x.Email ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                   (x.AccessLevel.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString));
            }

            if (model.Type == (int)StaffSortType.Archived)
            {
                staffQuery = staffQuery.Where(x => x.IsArchived);
            }
            else if (model.Type == (int)StaffSortType.Active)
            {
                staffQuery = staffQuery.Where(x => !x.IsArchived);
            }

            var totalRecords = await staffQuery.CountAsync();

            staffQuery = model.SortBy switch
            {
                (int)StaffSortBy.Email => staffQuery.OrderBy(x => x.Email),
                (int)StaffSortBy.UserName => staffQuery.OrderBy(x => x.UserName),
                _ => staffQuery.OrderByDescending(x => x.UpdatedDate)
            };

            var staffs = await staffQuery
                .Skip(model.PageSize * (model.PageIndex - 0))
                .Take(model.PageSize)
                .Select(s => new GetStaffModel
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
                .ToListAsync();

            return new PagedResponse<List<GetStaffModel>>
            {
                Data = staffs,
                HasNextPage = totalRecords > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = totalRecords,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200,
            };
        }

        //public async Task<PagedResponse<List<GetStaffModel>>> GetAllStaff(GetAllStaffInputModel model)
        //{
        //    model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

        //    var staffQuery = _unitOfWork.UserRepository.FindByCondition(x => x.IsActive && x.UserRole.FirstOrDefault().RoleId == (long)Roles.Staff).AsQueryable();

        //    staffQuery = !string.IsNullOrWhiteSpace(model.SearchString) ?
        //                 staffQuery.Where(x => (x.FirstName + x.LastName ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString)) : staffQuery;


        //    staffQuery = model.Type == (int)StaffSortType.Archived
        //               ? staffQuery.Where(x => x.IsArchived)
        //               : (model.Type == (int)StaffSortType.Active
        //               ? staffQuery.Where(x => x.IsArchived == false)
        //               : staffQuery);

        //    staffQuery = model.SortBy == (int)StaffSortBy.All
        //                                ? staffQuery.OrderByDescending(x => x.UpdatedDate)
        //                                : (model.SortBy == (int)StaffSortBy.Email
        //                                ? staffQuery.OrderBy(x => x.Email)
        //                                : staffQuery.OrderBy(x => x.UserName));

        //    var staffs = await staffQuery.GroupBy(x => 1)
        //                                 .Select(x => new PagedResponseOutput<List<GetStaffModel>>
        //                                 {
        //                                     TotalCount = x.Count(),
        //                                     Data = x.Select(s => new GetStaffModel
        //                                     {
        //                                         UserId = s.Id,
        //                                         UserName = s.UserName,
        //                                         FirstName = s.FirstName,
        //                                         LastName = s.LastName,
        //                                         Email = s.Email,
        //                                         IsArchived = s.IsArchived,
        //                                         IsApproved = s.IsApproved,
        //                                         MobileNumber = s.MobileNumber,
        //                                         AccessLevelId = s.AccessLevelId,
        //                                         AccessLevelName = s.AccessLevel.Name,
        //                                         DepartmentId = s.DepartmentId,
        //                                         DepartmentName = s.Department.Name,
        //                                         ProfilePicture = s.ProfilePicture,
        //                                         CreatedDate = s.CreatedDate,
        //                                         UpdatedDate = s.UpdatedDate
        //                                     })
        //                                             .Skip(model.PageSize * (model.PageIndex - 0))
        //                                             .Take(model.PageSize)
        //                                             .ToList()
        //                                 }).FirstOrDefaultAsync();


        //    return new PagedResponse<List<GetStaffModel>>
        //    {
        //        Data = staffs?.Data ?? [],
        //        HasNextPage = staffs?.TotalCount > (model.PageSize * model.PageIndex),
        //        HasPreviousPage = model.PageIndex > 1,
        //        TotalRecords = staffs == null ? 0 : staffs.TotalCount,
        //        SearchString = model.SearchString,
        //        PageSize = model.PageSize,
        //        PageIndex = model.PageIndex,
        //        Message = ResponseConstants.Success,
        //        StatusCode = 200,
        //    };
        //}

        public async Task<ApiResponse> ApproveBadge(ApproveBadgeModel model)
        {
            var badge = await _unitOfWork.BadgeRepository
                                         .FindByCondition(x => model.BadgeIds.Contains(x.Id) &&
                                        x.IsActive && x.IsApproved == false)
                                         .ToListAsync();

            if (badge.Count() <= 0)
                return new(400, ResponseConstants.BadRequest);

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
