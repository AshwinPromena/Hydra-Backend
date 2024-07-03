using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.BusinessLayer.Repository.IService.ILearnerService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hydra.BusinessLayer.Repository.Service.LearnerService
{
    public class LearnerManagmentService(IUnitOfWork unitOfWork,
                                         IReportService reportService,
                                         IBadgeService badgeService,
                                         IStorageService storageService,
                                         ICurrentUserService currentUserService) : ILearnerManagmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IReportService _reportService = reportService;
        private readonly IBadgeService _badgeService = badgeService;
        private readonly IStorageService _storageService = storageService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ServiceResponse<LearnerDashBoardModel>> LearnerDashBoard()
        {
            var learnerCount = await _unitOfWork.UserRepository
                                                .FindByCondition(x => x.UserRole.FirstOrDefault().RoleId == (long)Roles.Learner &&
                                                                      x.IsActive)
                                                .Include(i => i.LearnerBadge)
                                                .ThenInclude(ti => ti.Badge)
                                                .Select(x => new
                                                {
                                                    x.Id,
                                                    badgeCount = x.LearnerBadge.Where(x => x.IsActive).Count(),
                                                    x.CreatedDate,
                                                })
                                                .ToListAsync();

            var badgeType = await _unitOfWork.LearnerBadgeRepository.FindByCondition(x => x.IsActive).Include(i => i.Badge).ToListAsync();
            return new(200, ResponseConstants.Success, new LearnerDashBoardModel
            {
                LearnerInTotal = learnerCount.Count,
                LearnerWithBadge = learnerCount.Where(x => x.badgeCount != 0).Count(),
                LearnerWithoutBadge = learnerCount.Where(x => x.badgeCount == 0).Count(),
                AddedTodayCount = learnerCount.Where(x => x.CreatedDate.Date == DateTime.UtcNow.Date).Count(),
                LearnerBadgeCount = badgeType.Where(x => x.Badge.BadgeTypeId == (long)BadgeSortBy.Badge).Count(),
                LearnerCertificateCount = badgeType.Where(x => x.Badge.BadgeTypeId == (long)BadgeSortBy.Certificate).Count(),
                LearnerLicenseCount = badgeType.Where(x => x.Badge.BadgeTypeId == (long)BadgeSortBy.License).Count(),
                LearnerMiscellaneousCount = badgeType.Where(x => x.Badge.BadgeTypeId == (long)BadgeSortBy.Miscellaneous).Count(),
            });

        }

        public async Task<ServiceResponse<List<ExistingLearnerModel>>> BatchUploadLeraner(List<AddLearnerModel> model)
        {
            var verifyLearner = await _unitOfWork.UserRepository.FindByCondition(l => model.Select(s => s.Email).Contains(l.Email)).Select(s => s.Email).ToListAsync();
            var newLearners = model.Where(data => !verifyLearner.Contains(data.Email)).ToList();
            if (verifyLearner.Count > 0)
            {
                return new ServiceResponse<List<ExistingLearnerModel>>
                {
                    Data = verifyLearner.Select(email => new ExistingLearnerModel { Email = email }).ToList(),
                    Message = ResponseConstants.LearnersExists,
                    StatusCode = 409,
                };
            }
            else
            {
                var learners = new List<User>();
                foreach (var user in model)
                {
                    var newUser = new User
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Email2 = user.Email2,
                        Email3 = user.Email3,
                        MobileNumber = user.MobileNumber,
                        IsApproved = true,
                        LearnerId = user.LearnerId,
                        UserRole = new List<UserRole>
                        {
                            new UserRole
                            {
                                 RoleId = (long)Roles.Learner,
                            }
                        }
                    };
                    learners.Add(newUser);
                }
                await _unitOfWork.UserRepository.CreateRange(learners);
                await _unitOfWork.UserRepository.CommitChanges();
                return new(200, ResponseConstants.LearnersAdded.Replace("{count}", newLearners.Count.ToString()));
            }
        }

        public async Task<ServiceResponse<long>> AddLearner(AddLearnerModel model)
        {
            var verifyLearner = await _unitOfWork.UserRepository.FindByCondition(x => x.Email == model.Email && x.IsActive).FirstOrDefaultAsync();
            if (verifyLearner != null)
                return new(400, ResponseConstants.LearnerExists);

            var learner = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Email2 = model.Email2,
                Email3 = model.Email3,
                MobileNumber = model.MobileNumber,
                IsApproved = true,
                LearnerId = model.LearnerId,
            };
            learner.UserRole.Add(new()
            {
                RoleId = (long)Roles.Learner,
                UserId = learner.Id,
            });
            learner.ProfilePicture = (await _storageService.UploadFile(FileExtentionService.GetMediapath(), model.ProfilePicture)).Data;
            await _unitOfWork.UserRepository.Create(learner);
            await _unitOfWork.UserRepository.CommitChanges();
            return new(200, ResponseConstants.LearnerAdded, learner.Id);
        }

        public async Task<ServiceResponse<GetS3UrlModel>> DownloadSampleExcelFile()
        {
            var data = new List<SampleExcelDataModel>()
            {
                new SampleExcelDataModel
                {
                    FirstName = "Mary",
                    LastName = "Johnson",
                    Email = "maryjohnson@yopmail.com",
                    Email2 = "johson@yopmail.com",
                    Email3 = "mary@yopmail.com",
                    MobileNumber ="(555) 123-4567",
                    LearnerId = "AD1234",
                }
            };
            List<Dictionary<string, string>> convertedList = new List<Dictionary<string, string>>();

            foreach (var order in data)
            {
                Dictionary<string, string> convertedDict = new Dictionary<string, string>();

                var orderProperties = order.GetType().GetProperties();
                foreach (var prop in orderProperties)
                {
                    string key = prop.Name;
                    string value = prop.GetValue(order, null)?.ToString() ?? "";
                    convertedDict.Add(key, value);
                }
                convertedList.Add(convertedDict);
            }
            var excelString = string.Empty;

            excelString = _reportService.DownloadExcelFromJson(convertedList);

            var s3Url = (await _storageService.UploadFile(FileExtentionService.GetMediapath(), excelString)).Data;

            return new(200, ResponseConstants.Success, new GetS3UrlModel
            {
                S3Url = s3Url
            });
        }

        public async Task<PagedResponse<List<GetLearnerModel>>> GetAllLearners(GetAllLearnerInputModel model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var learnersQuery = _unitOfWork.UserRepository
                                        .FindByCondition(x => x.IsActive && x.UserRole.FirstOrDefault().RoleId == (int)Roles.Learner);
            learnersQuery = !string.IsNullOrWhiteSpace(model.SearchString) ?
                            learnersQuery.Where(x => (x.FirstName + x.LastName ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                     (x.Email ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString)) : learnersQuery;

            learnersQuery = model.Type == (int)GetLearnerType.All
                          ? learnersQuery
                          : (model.Type == (int)GetLearnerType.Assigned
                          ? learnersQuery.Where(x => x.LearnerBadge.Where(x => x.IsActive && x.IsRevoked == false).Count() > 0)
                          : learnersQuery.Where(x => x.LearnerBadge.Where(x => x.IsActive).Count() == 0));

            learnersQuery = model.FromDate != null ? learnersQuery.Where(x => x.CreatedDate.Date >= model.FromDate.Value.Date) : learnersQuery;
            learnersQuery = model.ToDate != null ? learnersQuery.Where(x => x.CreatedDate.Date <= model.ToDate.Value.Date) : learnersQuery;
            var learners = await learnersQuery
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<GetLearnerModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderByDescending(x => x.CreatedDate)
                                                    .Select(s => new GetLearnerModel
                                                    {
                                                        UserId = s.Id,
                                                        Name = $"{s.FirstName} {s.LastName}",
                                                        FirstName = s.FirstName,
                                                        LastName = s.LastName,
                                                        Email = s.Email,
                                                        Email2 = s.Email2,
                                                        Email3 = s.Email3,
                                                        MobileNumber = s.MobileNumber,
                                                        LearnerBadgeModel = s.LearnerBadge
                                                                             .Where(a => a.IsActive)
                                                                             .Select(s => new LearnerBadgeModel
                                                                             {
                                                                                 BadgeId = s.BadgeId,
                                                                                 BadgeName = s.Badge.Name,
                                                                                 DepartmentId = s.Badge.DepartmentId,
                                                                                 DepartmentName = s.Badge.Department.Name,
                                                                                 IssuedDate = s.Badge.IssueDate,
                                                                                 ExpirationDate = s.Badge.ExpirationDate,
                                                                                 SequenceId = s.Badge.BadgeSequenceId,
                                                                                 SequenceName = s.Badge.BadgeSequence.Name,
                                                                                 BadgeTypeId = s.Badge.BadgeTypeId,
                                                                                 BadgeTypeName = s.Badge.BadgeType.Name,
                                                                             }).ToList(),
                                                        ProfilePicture = s.ProfilePicture,
                                                        LearnerId = s.LearnerId,
                                                    })
                                                    .Skip(model.PageSize * (model.PageIndex - 0))
                                                    .Take(model.PageSize)
                                                    .ToList()

                                        }).FirstOrDefaultAsync();

            return new PagedResponse<List<GetLearnerModel>>
            {
                Data = learners?.Data ?? [],
                HasNextPage = learners?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = learners?.TotalCount ?? 0,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse<GetLearnerByIdModel>> GetLearnerById(long userId)
        {
            return new(200, ResponseConstants.Success, await _unitOfWork.UserRepository
                                                                        .FindByCondition(x => x.Id == userId && x.IsActive)
                                                                        .Select(s => new GetLearnerByIdModel
                                                                        {
                                                                            UserId = s.Id,
                                                                            Name = $"{s.FirstName} {s.LastName}",
                                                                            FirstName = s.FirstName,
                                                                            LastName = s.LastName,
                                                                            Email = s.Email,
                                                                            Email2 = s.Email2,
                                                                            Email3 = s.Email3,
                                                                            MobileNumber = s.MobileNumber,
                                                                            LearnerBadgeModel = s.LearnerBadge.Where(x => x.IsActive && x.IsRevoked == false).Select(s => new LearnerBadgeModel
                                                                            {
                                                                                BadgeId = s.BadgeId,
                                                                                BadgeName = s.Badge.Name,
                                                                                DepartmentId = s.Badge.DepartmentId,
                                                                                DepartmentName = s.Badge.Department.Name,
                                                                                IssuedDate = s.Badge.IssueDate,
                                                                                ExpirationDate = s.Badge.ExpirationDate,
                                                                                SequenceId = s.Badge.BadgeSequenceId,
                                                                                SequenceName = s.Badge.BadgeSequence.Name,
                                                                                BadgeTypeId = s.Badge.BadgeTypeId,
                                                                                BadgeTypeName = s.Badge.BadgeType.Name
                                                                            }).ToList(),
                                                                            ProfilePicture = s.ProfilePicture,
                                                                            LearnerId = s.LearnerId,
                                                                            Active = s.LearnerBadge.Where(x => x.Badge.ExpirationDate >= DateTime.UtcNow && x.IsActive && x.IsRevoked == false).ToList().Count,
                                                                            Expiring = s.LearnerBadge.Where(x => x.Badge.IssueDate <= DateTime.UtcNow && x.Badge.ExpirationDate > DateTime.UtcNow && x.IsActive && x.IsRevoked == false).ToList().Count,
                                                                            Expired = s.LearnerBadge.Where(x => x.Badge.ExpirationDate < DateTime.UtcNow && x.IsActive && x.IsRevoked == false).ToList().Count,
                                                                            GetActiveCredentialModel = s.LearnerBadge.Where(x => x.Badge.IssueDate <= DateTime.UtcNow &&
                                                                                                       x.Badge.ExpirationDate >= DateTime.UtcNow &&
                                                                                                       x.IsActive &&
                                                                                                       x.IsRevoked == false)
                                                                                                        .Select(s => new GetActiveCredentialModel
                                                                                                        {
                                                                                                            BadgeId = s.BadgeId,
                                                                                                            BadgeName = s.Badge.Name,
                                                                                                            DepartmentId = s.Badge.DepartmentId,
                                                                                                            DepartmentName = s.Badge.Department.Name,
                                                                                                            IssuedDate = s.Badge.IssueDate,
                                                                                                            ExpirationDate = s.Badge.ExpirationDate,
                                                                                                            SequenceId = s.Badge.BadgeSequenceId,
                                                                                                            SequenceName = s.Badge.BadgeSequence.Name,
                                                                                                            BadgeTypeId = s.Badge.BadgeTypeId,
                                                                                                            BadgeTypeName = s.Badge.BadgeType.Name
                                                                                                        }).ToList(),
                                                                            GetExpirinyCredentialModel = s.LearnerBadge.Where(x => x.Badge.IssueDate <= DateTime.UtcNow &&
                                                                                                         x.Badge.ExpirationDate > DateTime.UtcNow &&
                                                                                                         x.IsActive &&
                                                                                                         x.IsRevoked == false)
                                                                                                          .Select(s => new GetExpirinyCredentialModel
                                                                                                          {
                                                                                                              BadgeId = s.BadgeId,
                                                                                                              BadgeName = s.Badge.Name,
                                                                                                              DepartmentId = s.Badge.DepartmentId,
                                                                                                              DepartmentName = s.Badge.Department.Name,
                                                                                                              IssuedDate = s.Badge.IssueDate,
                                                                                                              ExpirationDate = s.Badge.ExpirationDate,
                                                                                                              SequenceId = s.Badge.BadgeSequenceId,
                                                                                                              SequenceName = s.Badge.BadgeSequence.Name,
                                                                                                              BadgeTypeId = s.Badge.BadgeTypeId,
                                                                                                              BadgeTypeName = s.Badge.BadgeType.Name
                                                                                                          }).ToList(),
                                                                            GetexpiredCredentialModel = s.LearnerBadge.Where(x => x.Badge.ExpirationDate < DateTime.UtcNow &&
                                                                                                        x.IsActive &&
                                                                                                        x.IsRevoked == false)
                                                                                                         .Select(s => new GetexpiredCredentialModel
                                                                                                         {
                                                                                                             BadgeId = s.BadgeId,
                                                                                                             BadgeName = s.Badge.Name,
                                                                                                             DepartmentId = s.Badge.DepartmentId,
                                                                                                             DepartmentName = s.Badge.Department.Name,
                                                                                                             IssuedDate = s.Badge.IssueDate,
                                                                                                             ExpirationDate = s.Badge.ExpirationDate,
                                                                                                             SequenceId = s.Badge.BadgeSequenceId,
                                                                                                             SequenceName = s.Badge.BadgeSequence.Name,
                                                                                                             BadgeTypeId = s.Badge.BadgeTypeId,
                                                                                                             BadgeTypeName = s.Badge.BadgeType.Name
                                                                                                         }).ToList()
                                                                        }).FirstOrDefaultAsync());
        }

        public async Task<ApiResponse> UpdateLearner(UpdateLearnerModel model)
        {
            var verifyLearner = await _unitOfWork.UserRepository.FindByCondition(x => x.Id == model.UserId).FirstOrDefaultAsync();

            if (verifyLearner == null)
                return new(400, ResponseConstants.InvalidUserId);

            verifyLearner.FirstName = model.FirstName;
            verifyLearner.LastName = model.LastName;
            verifyLearner.Email = model.Email;
            verifyLearner.Email2 = model.Email2;
            verifyLearner.Email3 = model.Email3;
            verifyLearner.LearnerId = model.LearnerId;

            verifyLearner.ProfilePicture = !string.IsNullOrEmpty(model.ProfilePicture)
                                           ? (await _storageService.UploadFile(FileExtentionService.GetMediapath(), model.ProfilePicture)).Data
                                           : verifyLearner.ProfilePicture;
            _unitOfWork.UserRepository.Update(verifyLearner);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.LearnerUpdated);
        }

        public async Task<ApiResponse> RemoveProfilePicture(long userId)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(x => x.Id == userId && x.IsActive).FirstOrDefaultAsync();
            if (user == null)
                return new(200, ResponseConstants.InvalidUserId);

            user.ProfilePicture = null;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.Success);
        }

        public async Task<ApiResponse> RevokeBadgeFromLearner(RevokeBadgeModel model)
        {
            var learnerWithBadge = await _unitOfWork.LearnerBadgeRepository
                                                    .FindByCondition(x => model.UserIds.Contains(x.UserId) &&
                                                   x.User.IsActive &&
                                               model.BadgeIds.Contains(x.BadgeId) &&
                                                   x.IsRevoked == false)
                                                    .ToListAsync();
            if (learnerWithBadge == null)
                return new(400, ResponseConstants.InvalidBadgeId);

            learnerWithBadge.ForEach(x =>
            {
                x.IsRevoked = true;
                x.IsActive = false;
                x.UpdatedDate = DateTime.UtcNow;
            });

            _unitOfWork.LearnerBadgeRepository.UpdateRange(learnerWithBadge);
            await _unitOfWork.LearnerBadgeRepository.CommitChanges();
            return new(200, ResponseConstants.BadgeRevoked);
        }

        public async Task<ApiResponse> RemoveBadge(RemoveBadgeModel model)
        {
            var learnerWithBadge = await _unitOfWork.LearnerBadgeRepository
                                                    .FindByCondition(x => model.UserIds.Contains(x.UserId) &&
                                                    model.BadgeIds.Contains(x.BadgeId) && x.IsActive)
                                            .ToListAsync();

            if (learnerWithBadge == null)
                return new(400, ResponseConstants.InvalidBadgeId);

            learnerWithBadge.ForEach(x =>
            {
                x.IsActive = false;
                x.UpdatedDate = DateTime.UtcNow;
            });

            _unitOfWork.LearnerBadgeRepository.UpdateRange(learnerWithBadge);
            await _unitOfWork.LearnerBadgeRepository.CommitChanges();

            return new(200, ResponseConstants.BadgeRemoved);
        }

        public async Task<ApiResponse> RemoveLearners(List<RemoveLearnerModel> model)
        {
            var learners = await _unitOfWork.UserRepository
                                            .FindByCondition(x => model.Select(s => s.UserIds).Contains(x.Id) &&
                                           x.UserRole.FirstOrDefault().RoleId == (int)Roles.Learner)
                                            .Include(i => i.DeletedUser).AsNoTracking()
                                            .ToListAsync();

            if (learners.IsNullOrEmpty())
                return new(400, ResponseConstants.InvalidUserId);

            foreach (var learner in learners)
            {
                if (learner.IsActive)
                {
                    var currentDate = DateTime.UtcNow;
                    learner.IsActive = false;
                    learner.UpdatedDate = currentDate;

                    var reasonModel = model.FirstOrDefault(m => m.UserIds == learner.Id);
                    if (reasonModel is not null)
                    {
                        learner.DeletedUser.Add(new DeletedUser
                        {
                            UserId = _currentUserService.UserId,
                            Name = _currentUserService.Name,
                            Email = _currentUserService.Email,
                            DeletedUserId = learner.Id,
                            Reason = reasonModel.Reason,
                            DeletedDate = currentDate,
                            DeletedUserName = $"{learner.FirstName} {learner.LastName}",
                            DeletedUserEmail = learner.Email,
                        });
                    }
                }
            }

            _unitOfWork.UserRepository.UpdateRange(learners);
            await _unitOfWork.UserRepository.CommitChanges();

            return new(200, ResponseConstants.LearnersRemoved);
        }
    }
}
