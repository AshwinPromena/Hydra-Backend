using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.BusinessLayer.Concrete.Service.BadgeService
{
    public class BadgeService(IUnitOfWork unitOfWork, IStorageService storageService, ICurrentUserService currentUserService) : IBadgeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly IStorageService _storageService = storageService;

        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ApiResponse> AddBadge(AddBadgeModel model)
        {
            var badge = await _unitOfWork.BadgeRepository.FindByCondition(x => x.Name.ToLower().Replace(" ", string.Empty) == model.BadgeName.ToLower().Replace(" ",string.Empty) && x.IsActive).FirstOrDefaultAsync();
            if (badge != null)
                return new(400, ResponseConstants.BadgeExists);

            badge = new()
            {
                Name = model.BadgeName,
                Description = model.BadgeDescription,
                DepartmentId = model.DepartmentId,
                BadgeSequenceId = model.BadgeSequenceId,
                IssueDate = model.IssueDate,
                ExpirationDate = model.ExpirationDate,
                IsActive = true,
                IsApproved = !model.IsRequiresApproval,
                RequiresApproval = model.IsRequiresApproval,
                ApprovalUserId = model.ApprovalUserId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
            model.LearningOutcomes.ForEach(x => badge.BadgeField.Add(new()
            {
                Name = x.FieldName,
                Content = x.FieldContent,
                Type = (int) FieldType.LearningOutcomes,
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
                           ? ResponseConstants.RequiresApprovalBadgeAdded.Replace("{BadgeName}",badge.Name)
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

            if (!string.IsNullOrEmpty(model.BadgeImage))
            {
                if (!string.IsNullOrEmpty(badge.Image))
                    await _storageService.DeleteFile(badge.Image);

                badge.Image = _storageService.UploadFile(ResponseConstants.Mediapath, model.BadgeImage).Result.Data;
            }

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
                                                             UpdatedDate = b.UpdatedDate
                                                         }).FirstOrDefaultAsync();
            if (badge is null)
                return new(404, ResponseConstants.InvalidBadgeId);

            return new(200, ResponseConstants.Success, badge);
        }

        public async Task<PagedResponse<List<GetBadgeModel>>> GetAllBadges(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var data = await _unitOfWork.BadgeRepository
                                        .FindByCondition(x => x.IsActive)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                    (x.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                    (x.Description ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                    (x.Department.Name ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
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
                                                                      (string.IsNullOrEmpty(b.ApprovalUser.LastName) ? "" : b.ApprovalUser.LastName))
                                                    }).Skip(model.PageSize * (model.PageIndex - 1))
                                                      .Take(model.PageSize)
                                                      .ToList()
                                        }).FirstOrDefaultAsync();

            return new PagedResponse<List<GetBadgeModel>>
            {
                Data = data?.Data ?? [],
                HasNextPage = data?.TotalCount > (model.PageSize * model.PageIndex),
                HasPreviousPage = model.PageIndex > 1,
                TotalRecords = data == null ? 0 : data.TotalCount,
                SearchString = model.SearchString,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Message = ResponseConstants.Success,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse> AssignBadges(AssignBadgeModel model)
        {
            var currentUserId = _currentUserService.UserId;
            var dateTime = DateTime.UtcNow;
            var learnerBadges = await _unitOfWork.LearnerBadgeRepository.FindByCondition(x => model.UserIds.Contains(x.UserId) && x.IsActive).ToListAsync();
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

            _unitOfWork.LearnerBadgeRepository.UpdateRange(learnerBadges);
            await _unitOfWork.LearnerBadgeRepository.CommitChanges();

            return new(200, ResponseConstants.Success);
        }
    }
}
