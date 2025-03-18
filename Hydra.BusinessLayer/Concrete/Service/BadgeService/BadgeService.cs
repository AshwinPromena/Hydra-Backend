using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.BusinessLayer.Concrete.Service.BadgeService
{
    public class BadgeService(IUnitOfWork unitOfWork, IStorageService storageService, ICurrentUserService currentUserService) : IBadgeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly IStorageService _storageService = storageService;

        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ServiceResponse<BadgeFactoryDashBoardModel>> BadgeFactoryDashBoard()
        {
            var userBadge = await _unitOfWork.BadgeRepository.FindByCondition(x => x.UniversityId == _currentUserService.UniversityId).ToListAsync();
            var badgeType = await _unitOfWork.LearnerBadgeRepository
                                            .FindByCondition(x => x.IsActive)
                                            .Include(i => i.Badge) // Correct way to include related entity
                                            .Where(v => v.User.UniversityId == _currentUserService.UniversityId) // Apply filtering separately
                                            .ToListAsync();
            var learnerCount = await _unitOfWork.UserRepository
                                     .FindByCondition(x => x.UserRole.FirstOrDefault().RoleId == (long)Roles.Learner &&
                                                           x.UniversityId == _currentUserService.UniversityId &&
                                                           x.IsActive)  
                                     .Select(x => new
                                     {
                                         x.Id,
                                         badgeCount = x.LearnerBadge.Count(b => b.IsActive),  
                                         x.CreatedDate,
                                     })
                                     .ToListAsync();

            return new(200, ResponseConstants.Success, new BadgeFactoryDashBoardModel()
            {
                TotalCredentials = userBadge.Count,
                ActiveCredentials = userBadge.Where(x => x.IsActive).Count(),
                RecentCredentials = userBadge.Where(x => x.CreatedDate.Date == DateTime.UtcNow.Date && x.IsActive).Count(),
                LearnerWithBadge = learnerCount.Where(x => x.badgeCount != 0).Count(),
                LearnerWithoutBadge = learnerCount.Where(x => x.badgeCount == 0).Count(),
                TotalLearnerCount = learnerCount.Count(),
                PendingApproval = userBadge.Where(x => x.IsApproved is false && x.IsActive).Count(),
                RecentAssignedCredentials = userBadge.Where(x => x.UpdatedDate.Date == DateTime.UtcNow.Date && x.IsActive).Count(),
                BadgeCount = userBadge.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Badge && x.IsActive).Count(),
                CertificateCount = userBadge.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Certificate && x.IsActive).Count(),
                LicenseCount = userBadge.Where(x => x.BadgeTypeId == (long)BadgeSortBy.License && x.IsActive).Count(),
                MiscellaneousCount = userBadge.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Miscellaneous && x.IsActive).Count(),
                LearnerBadgeCount = badgeType.Where(x => x.Badge.BadgeTypeId == (long)BadgeSortBy.Badge).Count(),
                LearnerCertificateCount = badgeType.Where(x => x.Badge.BadgeTypeId == (long)BadgeSortBy.Certificate).Count(),
                LearnerLicenseCount = badgeType.Where(x => x.Badge.BadgeTypeId == (long)BadgeSortBy.License).Count(),
                LearnerMiscellaneousCount = badgeType.Where(x => x.Badge.BadgeTypeId == (long)BadgeSortBy.Miscellaneous).Count(),
            });
        }

        public async Task<ApiResponse> AddBadge(AddBadgeModel model)
        {
            var badge = await _unitOfWork.BadgeRepository.FindByCondition(x => x.Name.ToLower().Replace(" ", string.Empty) == model.BadgeName.ToLower().Replace(" ", string.Empty) && x.IsActive).FirstOrDefaultAsync();
            if (badge != null)
                return new(400, ResponseConstants.BadgeExists);

            var verifyDepartment = await _unitOfWork.DepartmentRepository
                                                    .FindByCondition(x => x.Id == model.DepartmentId)
                                                    .FirstOrDefaultAsync();
            if (verifyDepartment is null)
            {
                var department = new Department { Name = model.DepartmentName };
                await _unitOfWork.DepartmentRepository.Create(department);
                await _unitOfWork.DepartmentRepository.CommitChanges();
                badge.DepartmentId = department.Id;
            }

            badge = new()
            {
                Name = model.BadgeName,
                Description = model.BadgeDescription,
                DepartmentId = model.DepartmentId,
                BadgeSequenceId = model.BadgeSequenceId == 0 ? null : model.BadgeSequenceId,
                IssueDate = model.IssueDate,
                ExpirationDate = model.ExpirationDate,
                IsActive = true,
                IsApproved = !model.IsRequiresApproval,
                RequiresApproval = model.IsRequiresApproval,
                ApprovalUserId = model.ApprovalUserId == 0 ? null : model.ApprovalUserId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                BadgeTypeId = model.BadgeTypeId,
                UniversityId= _currentUserService.UniversityId
            };
            model.LearningOutcomes.ForEach(x => badge.BadgeField.Add(new()
            {
                Name = x.FieldName,
                Content = x.FieldContent,
                Type = (int)FieldType.LearningOutcomes,
                TypeName = FieldType.LearningOutcomes.ToString()
            }));
            model.Competencies.ForEach(x => badge.BadgeField.Add(new()
            {
                Name = x.FieldName,
                Content = x.FieldContent,
                Type = (int)FieldType.Competencies,
                TypeName = FieldType.Competencies.ToString()
            }));

            if (!string.IsNullOrEmpty(model.BadgeImage))
                badge.Image = _storageService.UploadFile(ResponseConstants.Mediapath, model.BadgeImage).Result.Data;

            await _unitOfWork.BadgeRepository.Create(badge);
            await _unitOfWork.BadgeRepository.CommitChanges();

            return new(200, model.IsRequiresApproval
                           ? ResponseConstants.RequiresApprovalBadgeAdded.Replace("{BadgeName}", badge.Name)
                           : ResponseConstants.ApprovedBadgeAdded.Replace("{BadgeName}", badge.Name));
        }

        public async Task<ApiResponse> UpdateBadge(UpdateBadgeModel model)
        {
            var badge = await _unitOfWork.BadgeRepository.FindByCondition(x => x.Id == model.BadgeId && x.IsActive).FirstOrDefaultAsync();
            if (badge == null)
                return new(400, ResponseConstants.InvalidBadgeId);

            var existingBadge = await _unitOfWork.BadgeRepository.FindByCondition(x => x.Name.ToLower().Replace(" ", string.Empty) == model.BadgeName.ToLower().Replace(" ", string.Empty) && x.Id != model.BadgeId && x.IsActive).FirstOrDefaultAsync();
            if (existingBadge != null)
                return new(400, ResponseConstants.BadgeExists);

            badge.Name = model.BadgeName;
            badge.Description = model.BadgeDescription;
            badge.DepartmentId = model.DepartmentId;
            badge.BadgeSequenceId = model.BadgeSequenceId;
            badge.IssueDate = model.IssueDate;
            badge.ExpirationDate = model.ExpirationDate;
            badge.IsApproved = !model.IsRequiresApproval;
            badge.RequiresApproval = model.IsRequiresApproval;
            badge.ApprovalUserId = model.ApprovalUserId;
            badge.UpdatedDate = DateTime.UtcNow;
            badge.BadgeTypeId = model.BadgeTypeId;

            var badgeFieldsList = await _unitOfWork.BadgeFieldRepository.FindByCondition(x => x.BadgeId == model.BadgeId).ToListAsync();
            _unitOfWork.BadgeFieldRepository.DeleteRange(badgeFieldsList);
            model.LearningOutcomes.ForEach(x => badge.BadgeField.Add(new()
            {
                Name = x.FieldName,
                Content = x.FieldContent,
                Type = (int)FieldType.LearningOutcomes,
                TypeName = FieldType.LearningOutcomes.ToString()
            }));
            model.Competencies.ForEach(x => badge.BadgeField.Add(new()
            {
                Name = x.FieldName,
                Content = x.FieldContent,
                Type = (int)FieldType.Competencies,
                TypeName = FieldType.Competencies.ToString()
            }));

            badge.Image = !string.IsNullOrEmpty(model.BadgeImage)
                                           ? (await _storageService.UploadFile(FileExtentionService.GetMediapath(), model.BadgeImage)).Data
                                           : badge.Image;

            _unitOfWork.BadgeRepository.Update(badge);
            await _unitOfWork.BadgeRepository.CommitChanges();

            return new(200, ResponseConstants.BadgeUpdated);
        }

        public async Task<ApiResponse> DeleteBadge(DeleteBadgeModel model)
        {
            var badgeList = await _unitOfWork.BadgeRepository.FindByCondition(x => x.IsActive && model.BadgeIds.Contains(x.Id)).ToListAsync();

            foreach (var badge in badgeList)
            {
                badge.IsActive = false;
                badge.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.BadgeRepository.Update(badge);
            }

            await _unitOfWork.BadgeRepository.CommitChanges();
            return new ApiResponse(200, ResponseConstants.BadgeDeleted);
        }

        public async Task<ServiceResponse<GetBadgeModel>> GetBadgeById(long badgeId)
        {
            var badge = await _unitOfWork.BadgeRepository.FindByCondition(x => x.Id == badgeId && x.IsActive)
                                                         .Select(b => new GetBadgeModel()
                                                         {
                                                             BadgeId = b.Id,
                                                             BadgeName = b.Name,
                                                             BadgeDescription = b.Description,
                                                             DepartmentId = b.DepartmentId,
                                                             DepartmentName = b.Department.Name,
                                                             BadgeImage = b.Image,
                                                             BadgeSequenceId = b.BadgeSequenceId,
                                                             BadgeSequenceName = b.BadgeSequence.Name,
                                                             IssueDate = b.IssueDate,
                                                             ExpirationDate = b.ExpirationDate,
                                                             IsApproved = b.IsApproved,
                                                             IsRequiresApproval = b.RequiresApproval,
                                                             IsSequence = b.BadgeSequenceId != null,
                                                             ApprovalUserId = b.ApprovalUserId,
                                                             LearningOutcomes = b.BadgeField.Where(x => x.Type == (long)FieldType.LearningOutcomes)
                                                                                           .Select(a => new BadgeFieldModel()
                                                                                           {
                                                                                               FieldName = a.Name,
                                                                                               FieldContent = a.Content
                                                                                           }).ToList(),
                                                             Competencies = b.BadgeField.Where(x => x.Type == (long)FieldType.Competencies)
                                                                                           .Select(a => new BadgeFieldModel()
                                                                                           {
                                                                                               FieldName = a.Name,
                                                                                               FieldContent = a.Content
                                                                                           }).ToList(),
                                                             ApprovalUser = b.ApprovalUserId == null ? null :
                                                                            ((string.IsNullOrEmpty(b.ApprovalUser.FirstName) ? "" : b.ApprovalUser.FirstName) +
                                                                            (!string.IsNullOrEmpty(b.ApprovalUser.FirstName) && !string.IsNullOrEmpty(b.ApprovalUser.LastName) ? " " : "") +
                                                                            (string.IsNullOrEmpty(b.ApprovalUser.LastName) ? "" : b.ApprovalUser.LastName)),
                                                             CreatedDate = b.CreatedDate,
                                                             UpdatedDate = b.UpdatedDate,
                                                             BadgeTypeId = b.BadgeTypeId,
                                                             BadgeTypeName = b.BadgeType.Name,
                                                         }).FirstOrDefaultAsync();
            if (badge is null)
                return new(404, ResponseConstants.InvalidBadgeId);

            return new(200, ResponseConstants.Success, badge);
        }

        public async Task<PagedResponse<List<GetBadgeModel>>> GetAllBadges(GetAllBadgeInputModel model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);
            string Approved = "approved";
            string NotApproved = "notapproved";

            var badgesQuery = await _unitOfWork.BadgeRepository
                                         .FindByCondition(x => x.IsActive)
                                         .Include(i => i.Department)
                                         .Include(i => i.BadgeType)
                                         .Include(i => i.BadgeSequence)
                                         .Include(i => i.ApprovalUser)
                                         .ToListAsync();
            if (model.SearchString.ToLower().Replace(" ", string.Empty) != Approved && model.SearchString.ToLower().Replace(" ", string.Empty) != NotApproved)
            {
                badgesQuery = !string.IsNullOrWhiteSpace(model.SearchString) ?
                               badgesQuery.Where(x => (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                      (x.Description ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                      (x.Department.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                      (x.BadgeSequence?.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                      (x.BadgeType.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                      (x.IssueDate.ToString("dd-MM-yy").Contains(model.SearchString.Replace("/", "-")) ||
                                                      (x.ExpirationDate.HasValue && x.ExpirationDate.Value.ToString("dd-MM-yy").Contains(model.SearchString.Replace("/", "-"))))).ToList() : badgesQuery;
            }
            else
            {
                badgesQuery = badgesQuery.Where(x => model.SearchString.ToLower().Replace(" ", string.Empty) == Approved ? x.IsApproved : x.IsApproved == false).ToList();
            }



            badgesQuery = model.SortBy == (long)BadgeSortBy.All ? badgesQuery :
                          model.SortBy == (long)BadgeSortBy.Badge ? badgesQuery.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Badge).OrderByDescending(x => x.ExpirationDate).ToList() :
                          model.SortBy == (long)BadgeSortBy.Certificate ? badgesQuery.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Certificate).OrderByDescending(x => x.ExpirationDate).ToList() :
                          model.SortBy == (long)BadgeSortBy.License ? badgesQuery.Where(x => x.BadgeTypeId == (long)BadgeSortBy.License).OrderByDescending(x => x.ExpirationDate).ToList() :
                          model.SortBy == (long)BadgeSortBy.Miscellaneous ? badgesQuery.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Miscellaneous).OrderByDescending(x => x.ExpirationDate).ToList() :
                          badgesQuery.OrderByDescending(x => x.ExpirationDate).ToList();

            var badges = badgesQuery
                                          .GroupBy(x => 1)
                                          .Select(x => new PagedResponseOutput<List<GetBadgeModel>>
                                          {
                                              TotalCount = x.Count(),
                                              Data = x.OrderByDescending(x => x.CreatedDate)
                                                    .Select(b => new GetBadgeModel
                                                    {
                                                        BadgeId = b.Id,
                                                        BadgeName = b.Name,
                                                        BadgeDescription = b.Description,
                                                        DepartmentId = b.DepartmentId,
                                                        DepartmentName = b.Department?.Name,
                                                        BadgeSequenceId = b.BadgeSequenceId,
                                                        BadgeSequenceName = b.BadgeSequence?.Name,
                                                        IssueDate = b.IssueDate,
                                                        ExpirationDate = b.ExpirationDate,
                                                        IsApproved = b.IsApproved,
                                                        ApprovalUserId = b.ApprovalUserId,
                                                        IsRequiresApproval = b.RequiresApproval,
                                                        CreatedDate = b.CreatedDate,
                                                        UpdatedDate = b.UpdatedDate,
                                                        ApprovalUser = b.ApprovalUserId == null ? null :
                                                                                                        ((string.IsNullOrEmpty(b.ApprovalUser.FirstName) ? "" : b.ApprovalUser.FirstName) +
                                                                                                        (!string.IsNullOrEmpty(b.ApprovalUser.FirstName) && !string.IsNullOrEmpty(b.ApprovalUser.LastName) ? " " : "") +
                                                                                                        (string.IsNullOrEmpty(b.ApprovalUser.LastName) ? "" : b.ApprovalUser.LastName)),
                                                        BadgeImage = b.Image,
                                                        BadgeTypeId = b.BadgeTypeId,
                                                        BadgeTypeName = b.BadgeType?.Name,
                                                    })
                                                    .Skip(model.PageSize * (model.PageIndex - 0))
                                                    .Take(model.PageSize)
                                                    .ToList()

                                          }).FirstOrDefault();


            return new PagedResponse<List<GetBadgeModel>>
            {
                Data = badges?.Data ?? [],
                HasNextPage = badges?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = badges == null ? 0 : badges.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse<List<NotApprovedBadgeModel>>> AssignBadges(AssignBadgeModel model)
        {
            var currentUserId = _currentUserService.UserId;
            var dateTime = DateTime.UtcNow;
            var learnerBadges = await _unitOfWork.LearnerBadgeRepository.FindByCondition(x => model.UserIds.Contains(x.UserId) && x.IsActive).Include(i => i.Badge).ToListAsync();
            var notApprovedBadge = await _unitOfWork.BadgeRepository
                                                   .FindByCondition(x => model.BadgeIds.Contains(x.Id) && x.IsActive && x.IsApproved == false)
                                                   .Select(s => new NotApprovedBadgeModel
                                                   {
                                                       BadgeId = s.Id,
                                                       BadgeName = s.Name,
                                                   })
                                                   .ToListAsync();
            if (notApprovedBadge.Count == 0)
            {
                foreach (var userId in model.UserIds)
                {
                    foreach (var badgeId in model.BadgeIds)
                    {
                        if (!learnerBadges.Any(x => x.UserId == userId && x.BadgeId == badgeId))
                        {
                            learnerBadges.Add(new LearnerBadge
                            {
                                UserId = userId,
                                BadgeId = badgeId,
                                CreatedDate = dateTime,
                                UpdatedDate = dateTime,
                                IsActive = true,
                                IsRevoked = false,
                                IssuedBy = currentUserId
                            });
                        }
                    }
                }
            }
            else
            {
                return new ServiceResponse<List<NotApprovedBadgeModel>>(400, ResponseConstants.NotApprovedBadge, notApprovedBadge);
            }

            _unitOfWork.LearnerBadgeRepository.UpdateRange(learnerBadges);
            await _unitOfWork.LearnerBadgeRepository.CommitChanges();

            return new(200, ResponseConstants.Success);
        }

        public async Task<PagedResponse<List<GetBadgeModel>>> GetUnApprovedBadges(GetUnApprovedBadgeInputModel model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var badgesQuery = _unitOfWork.BadgeRepository
                                        .FindByCondition(x => x.IsActive && x.IsApproved == false);
            badgesQuery = !string.IsNullOrWhiteSpace(model.SearchString) ?
                           badgesQuery.Where(x => (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                  (x.Description ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                  (x.Department.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString)) : badgesQuery;

            badgesQuery = model.SortBy == (long)BadgeSortBy.All ? badgesQuery :
                          (model.SortBy == (long)BadgeSortBy.Badge ? badgesQuery.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Badge).OrderByDescending(x => x.ExpirationDate) :
                          (model.SortBy == (long)BadgeSortBy.Certificate ? badgesQuery.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Certificate).OrderByDescending(x => x.ExpirationDate) :
                          (model.SortBy == (long)BadgeSortBy.License ? badgesQuery.Where(x => x.BadgeTypeId == (long)BadgeSortBy.License).OrderByDescending(x => x.ExpirationDate) :
                          (model.SortBy == (long)BadgeSortBy.Miscellaneous ? badgesQuery.Where(x => x.BadgeTypeId == (long)BadgeSortBy.Miscellaneous).OrderByDescending(x => x.ExpirationDate) :
                          badgesQuery.OrderByDescending(x => x.ExpirationDate)))));

            var badges = await badgesQuery
                                          .GroupBy(x => 1)
                                          .Select(x => new PagedResponseOutput<List<GetBadgeModel>>
                                          {
                                              TotalCount = x.Count(),
                                              Data = x.OrderByDescending(x => x.UpdatedDate)
                                                    .Select(b => new GetBadgeModel
                                                    {
                                                        BadgeId = b.Id,
                                                        BadgeName = b.Name,
                                                        BadgeDescription = b.Description,
                                                        DepartmentId = b.DepartmentId,
                                                        DepartmentName = b.Department.Name,
                                                        BadgeSequenceId = b.BadgeSequenceId,
                                                        BadgeSequenceName = b.BadgeSequence.Name,
                                                        IssueDate = b.IssueDate,
                                                        ExpirationDate = b.ExpirationDate,
                                                        IsApproved = b.IsApproved,
                                                        ApprovalUserId = b.ApprovalUserId,
                                                        IsRequiresApproval = b.RequiresApproval,
                                                        CreatedDate = b.CreatedDate,
                                                        UpdatedDate = b.UpdatedDate,
                                                        ApprovalUser = b.ApprovalUserId == null ? null :
                                                                                                        ((string.IsNullOrEmpty(b.ApprovalUser.FirstName) ? "" : b.ApprovalUser.FirstName) +
                                                                                                        (!string.IsNullOrEmpty(b.ApprovalUser.FirstName) && !string.IsNullOrEmpty(b.ApprovalUser.LastName) ? " " : "") +
                                                                                                        (string.IsNullOrEmpty(b.ApprovalUser.LastName) ? "" : b.ApprovalUser.LastName)),
                                                        BadgeImage = b.Image,
                                                        BadgeTypeId = b.BadgeTypeId,
                                                        BadgeTypeName = b.BadgeType.Name,
                                                    })
                                                    .Skip(model.PageSize * (model.PageIndex - 0))
                                                    .Take(model.PageSize)
                                                    .ToList()

                                          }).FirstOrDefaultAsync();


            return new PagedResponse<List<GetBadgeModel>>
            {
                Data = badges?.Data ?? [],
                HasNextPage = badges?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = badges == null ? 0 : badges.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }

        public async Task<PagedResponse<List<GetBadgePicturesModel>>> GetBadgePictures(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var badges = await _unitOfWork.BadgeRepository
                                        .FindByCondition(x => x.IsActive)
                                        .Where(x => string.IsNullOrWhiteSpace(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<GetBadgePicturesModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderByDescending(x => x.UpdatedDate)
                                                    .Select(s => new GetBadgePicturesModel
                                                    {
                                                        BadgePictureUrl = s.Image,
                                                    }).Skip(model.PageSize * (model.PageIndex - 0))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();


            return new PagedResponse<List<GetBadgePicturesModel>>
            {
                Data = badges?.Data ?? [],
                HasNextPage = badges?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = badges == null ? 0 : badges.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }
    }
}
