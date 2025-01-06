using Hydra.BusinessLayer.Concrete.IService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Common.Repository.Service;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.BusinessLayer.Concrete.Service
{
    public class ContactSupportFormService(IUnitOfWork unitOfWork, IEmailService emailService) : IContactSupportFormService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEmailService _emailService = emailService;

        public async Task<ApiResponse> AddContactSupportForm(ContactSupportModel model)
        {
            ContactSupportForm contactSupportForm = new()
            {
                Name = model.Name,
                Email = model.Email,
                MobileNumber = model.MobileNumber,
                Description = model.Description,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.ContactSupportFormRepository.Create(contactSupportForm);
            await _unitOfWork.ContactSupportFormRepository.CommitChanges();
            await _emailService.SendContactSupportMail(model);
            return new(200, ResponseConstants.Success);
        }

        public async Task<PagedResponse<List<ContactSupportModel>>> GetAllContactSupportForm(PagedResponseInput model)
        {
            model.SearchString = string.IsNullOrEmpty(model.SearchString) ? null : model.SearchString.ToLower().Replace(" ", string.Empty);
            var records = await _unitOfWork.ContactSupportFormRepository
                         .FindByCondition(x => string.IsNullOrEmpty(model.SearchString) 
                                             ? true 
                                             : ((x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                (x.Email ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                (x.MobileNumber ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                (x.Description ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString)))
                         
                         .GroupBy(x => 1)
                         .Select(x => new PagedResponseOutput<List<ContactSupportModel>>
                         {
                             TotalCount = x.Count(),
                             Data = x.OrderByDescending(x => x.CreatedDate)
                                     .Skip(model.PageSize * model.PageIndex)
                                     .Take(model.PageSize)
                                     .Select(s => new ContactSupportModel
                                     {
                                         Name = s.Name,
                                         Email = s.Email,
                                         MobileNumber = s.MobileNumber,
                                         Description = s.Description
                                     })
                                     .ToList()
                         
                         }).FirstOrDefaultAsync();

            return new PagedResponse<List<ContactSupportModel>>
            {
                Data = records?.Data ?? [],
                HasNextPage = records?.TotalCount > (model.PageSize * (model.PageIndex + 1)),
                HasPreviousPage = model.PageIndex > 0,
                TotalRecords = records?.TotalCount ?? 0,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }
    }
}
