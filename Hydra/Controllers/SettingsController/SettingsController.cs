using Hydra.BusinessLayer.Concrete.IService.ISettingsService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.SettingsController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController(ISettingsService settingsService) : ControllerBase
    {
        private readonly ISettingsService _settingsService = settingsService;

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff , UniversityAdmin")]
        public async Task<ApiResponse> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _settingsService.ChangePassword(model);
        }

        [HttpPost("[action]"), Authorize(Roles = "Admin , Staff , UniversityAdmin")]
        public async Task<PagedResponse<List<GetAllDeletedUserModel>>> GetAllDeletedUser(GetAllDeletedUserInputModel model)
        {
            return await _settingsService.GetAllDeletedUser(model);
        }
    }
}
