using Hydra.BusinessLayer.Concrete.IService;
using Hydra.BusinessLayer.Concrete.Service;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.ContactSupportController
{
    [Route("api/[controller]")]
    [ApiController]

    public class ContactSupportController(IContactSupportFormService contactSupportFormService) : ControllerBase
    {
        private readonly IContactSupportFormService _contactSupportFormService = contactSupportFormService;

        [HttpPost("[action]")]
        public async Task<ApiResponse> ContactSupport(ContactSupportModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _contactSupportFormService.AddContactSupportForm(model);
        }

        [HttpPost("[action]"), Authorize]
        public async Task<PagedResponse<List<ContactSupportModel>>> GetAllContactSupportForm(PagedResponseInput model)
        {
            return await _contactSupportFormService.GetAllContactSupportForm(model);
        }
    }
}
