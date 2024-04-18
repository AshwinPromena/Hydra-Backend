using Hydra.BusinessLayer.Repository.IService.IDropDownService;
using Hydra.Common.Models;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.BusinessLayer.Repository.Service.DropDownService
{
    public class DropDownService(HydraContext context, IUnitOfWork unitOfWork) : IDropDownService
    {
        private readonly HydraContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ServiceResponse<List<DepartmentModel>>> GetAllDepartment()
        {
            var department = await _unitOfWork.DepartmentRepository.FindByCondition(x => x.Id > 0).Select(s => new DepartmentModel
            {
                DepartmentId = s.Id,
                DepartmentName = s.DepartmentName,
            }).ToListAsync();

            return new(200, ResponseConstants.Success, department);
        }


        public async Task<ServiceResponse<List<AccessLevelModel>>> GetAllAccessLevel()
        {
            var accessLevel = await _unitOfWork.AccessLevelRepository.FindByCondition(x => x.Id > 0).Select(s => new AccessLevelModel
            {
                AccessLevelId = s.Id,
                AccessLevelName = s.AccessLevelName,
            }).ToListAsync();
            return new(200, ResponseConstants.Success, accessLevel);
        }
    }
}
