using Hydra.BusinessLayer.Concrete.IService.IUniversityService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.BusinessLayer.Concrete.Service.UniversityService
{
    public class UniversityService(IUnitOfWork unitOfWork, IStorageService storageService) : IUniversityService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IStorageService _storageService = storageService;


        public async Task<ApiResponse> AddUniversity(AddUniversityModel model)
        {
            var existingUniversity = await _unitOfWork.UniversityRepository
                                                      .FindByCondition(x =>
                                                     x.Name == model.UniversityName)
                                                      .FirstOrDefaultAsync();

            if (existingUniversity is not null)
                return new(409, ResponseConstants.UniversityExists);

            var universityModel = new University
            {
                Name = model.UniversityName,
                LogoUrl = model.UniversityLogo,
                Color = model.Color,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };

            await _unitOfWork.UniversityRepository.Create(universityModel);
            await _unitOfWork.UniversityRepository.CommitChanges();

            return new(200, ResponseConstants.UniversityAdded);
        }


        public async Task<ServiceResponse<GetUniversityByIdModel>> GetUniversityById(long universityId)
        {
            return new(200, ResponseConstants.Success ,await _unitOfWork.UniversityRepository
                                                      .FindByCondition(x =>
                                                     x.Id == universityId)
                                                      .Select(s => new GetUniversityByIdModel
                                                      {
                                                          UniversityName = s.Name,
                                                          UniversityLogo = s.LogoUrl,
                                                          Color = s.Color,
                                                          IsActive = s.IsActive,
                                                          CreatedDate = s.CreatedDate,
                                                          UpdatedDate = s.UpdatedDate,
                                                          UniversityId = s.Id
                                                      })
                                                      .FirstOrDefaultAsync());
        }


        public async Task<PagedResponse<List<GetAllUniversityModel>>> GetAllUniversity(GetAllUniversityModel model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var universityQuery = _unitOfWork.UniversityRepository.FindByCondition(x => x.IsActive);

            universityQuery = !string.IsNullOrWhiteSpace(model.SearchString) ?
                              universityQuery.Where(x => (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                        (x.Color ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString)) : universityQuery;

            var universities = await universityQuery.GroupBy(x => 1)
                                                    .Select(s => new PagedResponseOutput<List<GetAllUniversityModel>>
                                                    {
                                                        TotalCount = s.Count(),
                                                        Data = s.OrderByDescending(x => x.UpdatedDate)
                                                                .Select(s => new GetAllUniversityModel
                                                                {
                                                                    UniversityId = s.Id,
                                                                    UniversityName = s.Name,
                                                                    UniversityLogo = s.LogoUrl,
                                                                    IsActive = s.IsActive,
                                                                    CreatedDate = s.CreatedDate,
                                                                    UpdatedDate = s.UpdatedDate,
                                                                })
                                                                .Skip(model.PageSize * (model.PageIndex - 0))
                                                                .Take(model.PageSize)
                                                                .ToList()
                                                    }).FirstOrDefaultAsync();

            return new PagedResponse<List<GetAllUniversityModel>>
            {
                Data = universities?.Data ?? [],
                HasNextPage = universities?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = universities?.TotalCount ?? 0,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }


        public async Task<ApiResponse> UpdateUniversity(UpdateUniversityModel model)
        {
            var existingUniversity = await _unitOfWork.UniversityRepository
                                          .FindByCondition(x =>
                                         x.Id == model.UniversityId)
                                          .FirstOrDefaultAsync();

            if (existingUniversity is null)
                return new(204, ResponseConstants.InvalidUserId);

            existingUniversity.Name = model.UniversityName;
            existingUniversity.Color = model.Color;
            existingUniversity.IsActive = true;
            existingUniversity.UpdatedDate = DateTime.UtcNow;

            existingUniversity.LogoUrl = !string.IsNullOrEmpty(model.UniversityLogo)
                                         ? (await _storageService.UploadFile(FileExtentionService.GetMediapath(), model.UniversityLogo)).Data
                                         : existingUniversity.LogoUrl;

            _unitOfWork.UniversityRepository.Update(existingUniversity);
            await _unitOfWork.UniversityRepository.CommitChanges();

            return new(200, ResponseConstants.UniversityUpdated);
        }


        public async Task<ApiResponse> DeleteUniversity(long universityId)
        {
            var existingUniversity = await _unitOfWork.UniversityRepository
                                          .FindByCondition(x =>
                                         x.Id == universityId)
                                          .FirstOrDefaultAsync();

            if (existingUniversity is null)
                return new(204, ResponseConstants.InvalidUserId);

            existingUniversity.IsActive = false;
            existingUniversity.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.UniversityRepository.Update(existingUniversity);
            await _unitOfWork.UniversityRepository.CommitChanges();

            return new(200, ResponseConstants.UniversityDeleted);
        }
    }
}
