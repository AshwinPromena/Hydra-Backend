using DocumentFormat.OpenXml.Spreadsheet;
using Hydra.BusinessLayer.Concrete.IService.IStaffService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.BusinessLayer.Concrete.Service.StaffService
{
    public  class StaffServices(IUnitOfWork unitOfWork, IStorageService storageService) : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly IStorageService _storageService = storageService;

        public async Task<ApiResponse> AddStaff(AddStaffModel model)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.UserName.ToLower().Equals(model.UserName.ToLower())).FirstOrDefaultAsync();
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
                IsApproved = true,
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

            if (!string.IsNullOrEmpty(model.ProfilePicture))
            {
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                    await _storageService.DeleteFile(user.ProfilePicture);
                user.ProfilePicture = _storageService.UploadFile(ResponseConstants.Mediapath, model.ProfilePicture).Result.Data;
            }

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new ApiResponse(200, ResponseConstants.StaffUpdated);
        }

        public async Task<ApiResponse> DeleteStaff(DeleteStaffModel model)
        {
            var userList = await _unitOfWork.UserRepository.FindByCondition(x => x.IsActive && model.UserIds.Contains(x.Id)).ToListAsync();

            foreach (var staff in userList)
            {
                staff.IsActive = false;
                staff.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.UserRepository.Update(staff);
            }

            await _unitOfWork.UserRepository.CommitChanges();
            return new ApiResponse(200, ResponseConstants.StaffDeleted);
        }

        public async Task<ApiResponse> ArchivedStaffs(DeleteStaffModel model)
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
                                            Data = x.OrderBy(x => x.UserName)
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
                                                    }).Skip(model.PageSize * (model.PageIndex - 1))
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
    }
}
