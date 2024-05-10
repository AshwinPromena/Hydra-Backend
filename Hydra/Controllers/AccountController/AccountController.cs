using Hydra.BusinessLayer.Repository.IService.IAccountService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HttpPost("[action]")]
        public async Task<ApiResponse> Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);
            return await _accountService.Register(model);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<LoginResponse>> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);
            return await _accountService.Login(model);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);
            return await _accountService.ForgotPassword(model);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> ValidateResetUrl(string token)
        {
            return await _accountService.ValidateResetUrl(token);
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<string>> ValidateOtp(ValidateOtpModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _accountService.ValidateOtp(model);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);
            return await _accountService.ResetPassword(model);
        }

        //[HttpPost("[action]")]
        //public async Task<ApiResponse> ReSendOtp(ForgotPasswordModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return new(400, ResponseConstants.BadRequest);
        //    return await _accountService.ReSendOtp(model);
        //}
    }
}
