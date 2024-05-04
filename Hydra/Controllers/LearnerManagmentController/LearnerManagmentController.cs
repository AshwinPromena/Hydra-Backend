using Hydra.BusinessLayer.Repository.IService.ILearnerService;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.LearnerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerManagmentController(ILearnerManagmentService learnerManagmentService) : ControllerBase
    {
        private readonly ILearnerManagmentService _learnerManagmentService = learnerManagmentService;


        [HttpGet("[action]")]
        public async Task<ServiceResponse<LearnerDashBoardModel>> LearnerDashBoard()
        {
            return await _learnerManagmentService.LearnerDashBoard();
        }

        [HttpGet("[action]")]
        public async Task<string> DownloadSampleExcelFile()
        {
            return await _learnerManagmentService.DownloadSampleExcelFile();
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<List<ExistingLearnerModel>>> BatchUploadLearner(IFormFile file)
        {
            if (file == null)
                return new(400, ResponseConstants.BadRequest);
            return await _learnerManagmentService.BatchUploadLeraner(file);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse> AddLearner(AddLearnerModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _learnerManagmentService.AddLearner(model);
        }


        [HttpPost("[action]"), Authorize(Roles = "Staff")]
        public async Task<ApiResponse> AssignBadgeToLearners(AssignBadgeModel model)
        {
            if (!ModelState.IsValid)
                return new(400, ResponseConstants.BadRequest);

            return await _learnerManagmentService.AssignBadgeToLearners(model);
        }
    }
}
