using Hydra.Common.Globle;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.ContactSupportController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactSupportController(IEmailService emailService) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;

        [HttpPost("[action]")]
        public async Task<ApiResponse> ContactSupport(ContactSupportModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _emailService.SendContactSupportMail(model);
        }
    }
}
