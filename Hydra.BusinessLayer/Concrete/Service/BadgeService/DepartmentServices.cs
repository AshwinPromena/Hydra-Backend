using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.BusinessLayer.Concrete.Service.BadgeService
{
    public class DepartmentServices(IUnitOfWork unitOfWork) : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ApiResponse> AddDepartment(string departmentName)
        {
            var department = await _unitOfWork.DepartmentRepository.FindByCondition(x => x.Name.ToLower().Replace(" ", string.Empty).Equals(departmentName.ToLower().Replace(" ", string.Empty)) && x.IsActive).FirstOrDefaultAsync();
            if (department is not null)
                return new(400, ResponseConstants.DepartmentExists);

            department = new()
            {
                Name = departmentName,
            };
            await _unitOfWork.DepartmentRepository.Create(department);
            await _unitOfWork.DepartmentRepository.CommitChanges();
            return new(200, ResponseConstants.DepartmentAdded);
        }


        public async Task<ApiResponse> UpdateDepartment(int departmentId, string departmentName)
        {
            var department = await _unitOfWork.DepartmentRepository.FindByCondition(x => x.Id == departmentId && x.IsActive).FirstOrDefaultAsync();
            if (department is  null)
                return new ApiResponse(404, ResponseConstants.InvalidDepartmentId);

            var existingDepartment = await _unitOfWork.DepartmentRepository
                                                      .FindByCondition(x => x.Name.ToLower().Replace(" ", string.Empty) == departmentName.ToLower().Replace(" ", string.Empty) && 
                                                                            x.IsActive && 
                                                                            x.Id != departmentId)
                                                      .FirstOrDefaultAsync();

            if (existingDepartment is not null)
                return new ApiResponse(400, ResponseConstants.DepartmentExists);

            department.Name = departmentName;
            department.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.DepartmentRepository.Update(department);
            await _unitOfWork.DepartmentRepository.CommitChanges();

            return new ApiResponse(200, ResponseConstants.DepartmentUpdated);
        }


        public async Task<ApiResponse> DeleteDepartment(int departmentId)
        {
            var department = await _unitOfWork.DepartmentRepository.FindByCondition(x => x.Id == departmentId && x.IsActive).FirstOrDefaultAsync();
            if (department is null)
                return new ApiResponse(404, ResponseConstants.InvalidDepartmentId);

            department.IsActive = false;
            department.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.DepartmentRepository.Update(department);
            await _unitOfWork.DepartmentRepository.CommitChanges();

            return new ApiResponse(200, ResponseConstants.DepartmentDeleted);
        }


        public async Task<ServiceResponse<DepartmentOutputModel>> GetDepartmentById(long departmentId)
        {
            var department = await _unitOfWork.DepartmentRepository.FindByCondition(x => x.Id == departmentId && x.IsActive)
                                                                   .Select(x => new DepartmentOutputModel()
                                                                   {
                                                                       DepartmentId = x.Id,
                                                                       DepartmentName = x.Name,
                                                                       CreatedDate = x.CreatedDate,
                                                                       UpdatedDate = x.UpdatedDate
                                                                   }).FirstOrDefaultAsync();
            if (department is null)
                return new(404, ResponseConstants.InvalidDepartmentId);
            return new(200, ResponseConstants.Success, department);
        }


        public async Task<PagedResponse<List<DepartmentOutputModel>>> GetAllDepartments(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var data = await _unitOfWork.DepartmentRepository
                .FindByCondition(x => x.IsActive)
                .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                            (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                .GroupBy(x => 1)
                .Select(x => new PagedResponseOutput<List<DepartmentOutputModel>>
                {
                    TotalCount = x.Count(),
                    Data = x.OrderBy(x => x.Name)
                            .Select(s => new DepartmentOutputModel
                            {
                                DepartmentId = s.Id,
                                DepartmentName = s.Name,
                                CreatedDate = s.CreatedDate,
                                UpdatedDate = s.UpdatedDate
                            }).Skip(model.PageSize * (model.PageIndex - 1))
                              .Take(model.PageSize)
                              .ToList()
                }).FirstOrDefaultAsync();

            return new PagedResponse<List<DepartmentOutputModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200,
            };
        }
    }

    public class DepartmentOutputModel
    {
        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }
}
