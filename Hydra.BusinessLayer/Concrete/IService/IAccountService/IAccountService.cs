using Hydra.Common.Models;

namespace Hydra.BusinessLayer.Repository.IService.IAccountService
{
    public interface IAccountService
    {
        Task<ApiResponse> Register(UserRegisterModel model);

        Task<ServiceResponse<LoginResponse>> Login(LoginModel model);

        Task<ApiResponse> ResetPassword(PasswordResetModel model);
    }
}
