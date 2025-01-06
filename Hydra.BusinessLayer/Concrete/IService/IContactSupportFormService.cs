using Hydra.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.BusinessLayer.Concrete.IService
{
    public interface IContactSupportFormService
    {
        Task<ApiResponse> AddContactSupportForm(ContactSupportModel model);

        Task<PagedResponse<List<ContactSupportModel>>> GetAllContactSupportForm(PagedResponseInput model);
    }
}
