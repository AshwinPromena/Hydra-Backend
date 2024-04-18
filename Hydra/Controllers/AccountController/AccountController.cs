using Hydra.BusinessLayer.Repository.IService.IAccountService;
using Hydra.BusinessLayer.Repository.IService.IDropDownService;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService, IDropDownService dropDownService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IDropDownService _dropDownService = dropDownService;

        [HttpPost("[action]")]
        public async Task<ApiResponse> Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return new(400, ResponseConstants.BadRequest);
            }
            return await _accountService.Register(model);
        }


        [HttpPost("[action]")]
        public async Task<ServiceResponse<LoginResponse>> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return new(400, ResponseConstants.BadRequest);
            }
            return await _accountService.Login(model);
        }


        [HttpPost("[action]")]
        public async Task<ApiResponse> ResetPassword(PasswordResetModel model)
        {
            if (!ModelState.IsValid)
            {
                return new(400, ResponseConstants.BadRequest);
            }
            return await _accountService.ResetPassword(model);
        }


        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<DepartmentModel>>> GetAllDepartment()
        {
            return await _dropDownService.GetAllDepartment();
        }


        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<AccessLevelModel>>> GetAllAccessLevel()
        {
            return await _dropDownService.GetAllAccessLevel();
        }
    }
}
