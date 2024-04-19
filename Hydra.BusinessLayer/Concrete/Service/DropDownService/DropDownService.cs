using Hydra.BusinessLayer.Repository.IService.IDropDownService;
using Hydra.Common.Models;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.BusinessLayer.Repository.Service.DropDownService
{
    public class DropDownService(IUnitOfWork unitOfWork) : IDropDownService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PagedResponse<List<DepartmentModel>>> GetAllDepartment(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);
            var data = await _unitOfWork.DepartmentRepository
                                        .FindByCondition(x => x.IsActive)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                   (x.DepartmentName ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<DepartmentModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderBy(x => x.DepartmentName)
                                                    .Select(s => new DepartmentModel
                                                    {
                                                        DepartmentId = s.Id,
                                                        DepartmentName = s.DepartmentName
                                                    }).Skip(model.PageSize * (model.PageIndex - 1))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();
            return new PagedResponse<List<DepartmentModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };
        }


        public async Task<PagedResponse<List<AccessLevelModel>>> GetAllAccessLevel(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);
            var data = await _unitOfWork.AccessLevelRepository
                                        .FindByCondition(x => x.Id > 0)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) || 
                                                   (x.Name?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<AccessLevelModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderBy(x => x.Name)
                                                    .Select(s => new AccessLevelModel
                                                    {
                                                        AccessLevelId = s.Id,
                                                        AccessLevelName = s.Name
                                                    }).Skip(model.PageSize * (model.PageIndex - 1))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();
            return new PagedResponse<List<AccessLevelModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };
        }
    }
}
