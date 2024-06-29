using Hydra.BusinessLayer.Concrete.IService.ISettingsService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.BusinessLayer.Concrete.Service.SettingsService
{
    public class SettingsService(ICurrentUserService currentUserService, IUnitOfWork unitOfWork) : ISettingsService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ApiResponse> ChangePassword(ChangePasswordModel model)
        {
            var user = await _unitOfWork.UserRepository
                                        .FindByCondition(x => x.Id == _currentUserService.UserId &&
                                       x.IsActive &&
                                       x.Password == model.OldPasssword &&
                                       x.UserRole.FirstOrDefault().RoleId == (long)Roles.Staff ||
                                       x.UserRole.FirstOrDefault().RoleId == (long)Roles.Admin)
                                        .FirstOrDefaultAsync();

            if (user is null)
                return new(400, ResponseConstants.BadRequest);

            user.Password = EncryptionService.Encipher(model.NewPassword);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.Password);
        }

        public async Task<PagedResponse<List<GetAllDeletedUserModel>>> GetAllDeletedUser(GetAllDeletedUserInputModel model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var userList = _unitOfWork.DeletedUserRepository.FindByCondition(x => x.Id > 0);

            userList = !string.IsNullOrWhiteSpace(model.SearchString) ?
                      userList.Where(x => (x.DeletedUserName ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                          (x.DeletedUserEmail ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString)) : userList;

            userList = model.TypeId == (long)DeletedUserOptions.All ? userList :
                       model.TypeId == (long)DeletedUserOptions.Learner ? userList.Where(x => x.User.UserRole.FirstOrDefault().RoleId == (int)Roles.Learner) :
                       model.TypeId == (long)DeletedUserOptions.Staff ? userList.Where(x => x.User.UserRole.FirstOrDefault().RoleId == (int)Roles.Staff)
                                       : userList;

            var deletedUsers = await userList
                                             .GroupBy(x => 1)
                                             .Select(x => new PagedResponseOutput<List<GetAllDeletedUserModel>>
                                             {
                                                 TotalCount = x.Count(),
                                                 Data = x.OrderByDescending(x => x.DeletedDate)
                                                         .Select(s => new GetAllDeletedUserModel
                                                         {
                                                             UserId = s.UserId,
                                                             Name = s.Name,
                                                             Email = s.Email,
                                                             DeletedUserId = s.DeletedUserId,
                                                             DeletedUserName = s.DeletedUserName,
                                                             DeletedUserEmail = s.DeletedUserEmail,
                                                             DeletdDate = s.DeletedDate,
                                                             ProfilePicture = s.User.ProfilePicture,
                                                             UserRole = s.User.UserRole.FirstOrDefault().Role.Name,
                                                         })
                                                         .Skip(model.PageSize * (model.PageIndex - 0))
                                                         .Take(model.PageSize)
                                                         .ToList()
                                             }).FirstOrDefaultAsync();

            return new PagedResponse<List<GetAllDeletedUserModel>>
            {
                Data = deletedUsers.Data ?? [],
                HasNextPage = deletedUsers?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = deletedUsers?.TotalCount ?? 0,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }
    }
}
