using Hydra.Common.Models;

namespace Hydra.BusinessLayer.Concrete.IService.ISettingsService
{
    public interface ISettingsService
    {
        Task<ApiResponse> ChangePassword(ChangePasswordModel model);

        Task<PagedResponse<List<GetAllDeletedUserModel>>> GetAllDeletedUser(GetAllDeletedUserInputModel model);
    }
}
