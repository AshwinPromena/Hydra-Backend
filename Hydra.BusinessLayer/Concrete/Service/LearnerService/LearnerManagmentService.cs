using ExcelDataReader;
using Hydra.BusinessLayer.Concrete.IService.IBadgeService;
using Hydra.BusinessLayer.Repository.IService.ILearnerService;
using Hydra.Common.Globle;
using Hydra.Common.Globle.Enum;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Hydra.Database.Entities;
using Hydra.DatbaseLayer.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hydra.BusinessLayer.Repository.Service.LearnerService
{
    public class LearnerManagmentService(IUnitOfWork unitOfWork, IReportService reportService, IBadgeService badgeService) : ILearnerManagmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IReportService _reportService = reportService;
        private readonly IBadgeService _badgeService = badgeService;


        public async Task<ServiceResponse<LearnerDashBoardModel>> LearnerDashBoard()
        {
            var learnerCount = await _unitOfWork.UserRepository
                                                .FindByCondition(x => x.UserRole.FirstOrDefault().RoleId == (long)Roles.Learner)
                                                .ToListAsync();
            return new(200, ResponseConstants.Success, new LearnerDashBoardModel
            {
                LearnerInTotal = learnerCount.Count,
                LearnerWithBadge = _unitOfWork.LearnerBadgeRepository.FindByCondition(x => x.IsActive).Count(),
                LearnerWithoutBadge = _unitOfWork.LearnerBadgeRepository.FindByCondition(x => x.IsActive == false).Count(),
            });

        }

        public async Task<ServiceResponse<List<ExistingLearnerModel>>> BatchUploadLeraner(IFormFile file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                IExcelDataReader reader;
                reader = ExcelReaderFactory.CreateReader(memoryStream);

                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };
                var dataSet = reader.AsDataSet(conf);
                var dataTable = dataSet.Tables[0];
                var json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
                List<User> learnerData = JsonConvert.DeserializeObject<List<User>>(json);
                var verifyLearner = await _unitOfWork.UserRepository.FindByCondition(l => learnerData.Select(s => s.Email).Contains(l.Email)).Select(s => s.Email).ToListAsync();
                var newLearners = learnerData.Where(data => !verifyLearner.Contains(data.Email)).ToList();
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
                    newLearners.ForEach(x => x.UserRole.Add(new()
                    {
                        RoleId = (long)Roles.Learner,
                        UserId = x.Id,
                    }));
                    await _unitOfWork.UserRepository.CreateRange(newLearners);
                    await _unitOfWork.UserRepository.CommitChanges();
                    return new(200, ResponseConstants.LearnersAdded.Replace("{count}", newLearners.Count().ToString()));
                }
            }
        }

        public async Task<ApiResponse> AddLearner(AddLearnerModel model)
        {
            var verifyLearner = await _unitOfWork.UserRepository.FindByCondition(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (verifyLearner != null)
                return new(400, ResponseConstants.LearnerExists);

            var learner = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };
            learner.UserRole.Add(new()
            {
                RoleId = (long)Roles.Learner,
                UserId = learner.Id,
            });
            await _unitOfWork.UserRepository.Create(learner);
            await _unitOfWork.UserRepository.CommitChanges();
            return new(200, ResponseConstants.LearnerAdded);
        }

        public async Task<string> DownloadSampleExcelFile()
        {
            var data = new List<SampleExcelDataModel>()
            {
                new SampleExcelDataModel
                {
                    FirstName = "Mary",
                    LastName = "Johnson",
                    Email = "maryjohnson@yopmail.com",
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

            excelString = await Task.Run(() => _reportService.DownloadExcelFromJson(convertedList));

            return excelString;
        }

        public async Task<ApiResponse> AssignBadgeToLearners(AssignBadgeModel model)
        {
            return await _badgeService.AssignBadges(model);
        }

        public async Task<PagedResponse<List<GetLearnerModel>>> GetAllLearners(PagedResponseInput model)
        {
            model.SearchString = model.SearchString.ToLower().Replace(" ", string.Empty);

            var data = await _unitOfWork.UserRepository
                                        .FindByCondition(x => x.IsActive && x.UserRole.FirstOrDefault().RoleId == (int)Roles.Learner)
                                        .Where(x => string.IsNullOrEmpty(model.SearchString) ||
                                                    (x.FirstName + x.LastName ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString) ||
                                                    (x.Email ?? string.Empty).ToLower().Replace(" ", string.Empty).Contains(model.SearchString))
                                        .GroupBy(x => 1)
                                        .Select(x => new PagedResponseOutput<List<GetLearnerModel>>
                                        {
                                            TotalCount = x.Count(),
                                            Data = x.OrderByDescending(x => x.CreatedDate)
                                                    .Select(s => new GetLearnerModel
                                                    {
                                                        UserId = x.FirstOrDefault().Id,
                                                        Name = x.FirstOrDefault().FirstName + x.FirstOrDefault().LastName,
                                                        Email = s.Email
                                                    })
                                                    .Skip(model.PageSize * (model.PageIndex - 1))
                                                    .Take(model.PageSize)
                                                    .ToList()

                                        }).FirstOrDefaultAsync();

            return new PagedResponse<List<GetLearnerModel>>
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

    }
}
