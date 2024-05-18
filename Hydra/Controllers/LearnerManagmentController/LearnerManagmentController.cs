using Hydra.BusinessLayer.Repository.IService.ILearnerService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.LearnerController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LearnerManagmentController(ILearnerManagmentService learnerManagmentService) : ControllerBase
    {
        private readonly ILearnerManagmentService _learnerManagmentService = learnerManagmentService;


        [HttpGet("[action]")]
        public async Task<ServiceResponse<LearnerDashBoardModel>> LearnerDashBoard()
        {
            return await _learnerManagmentService.LearnerDashBoard();
        }

        [HttpGet("[action]")]
        public async Task<ServiceResponse<GetS3UrlModel>> DownloadSampleExcelFile()
        {
            return await _learnerManagmentService.DownloadSampleExcelFile();
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<ExistingLearnerModel>>> BatchUploadLearner(List<AddLearnerModel> model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);
            return await _learnerManagmentService.BatchUploadLeraner(model);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> AddLearner(AddLearnerModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _learnerManagmentService.AddLearner(model);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> UpdateLearner(UpdateLearnerModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _learnerManagmentService.UpdateLearner(model);
        }

        [HttpPost("[action]")]
        public async Task<PagedResponse<List<GetLearnerModel>>> GetAllLearners(GetAllLearnerInputModel model)
        {
            return await _learnerManagmentService.GetAllLearners(model);
        }

        [HttpGet("[action]")]
        public async Task<ServiceResponse<GetLearnerByIdModel>> GetLearnerById(long userId)
        {
            return await _learnerManagmentService.GetLearnerById(userId);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> RevokeBadgeFromLearner(RevokeBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _learnerManagmentService.RevokeBadgeFromLearner(model);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> DeleteBadge(DeleteBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _learnerManagmentService.DeleteBadge(model);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> RemoveLearners(RemoveLearnerModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _learnerManagmentService.RemoveLearners(model);
        }
    }
}
