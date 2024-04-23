using Hydra.BusinessLayer.Repository.IService.ILearnerService;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Controllers.LearnerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerManagmentController(ILearnerManagmentService learnerManagmentService) : ControllerBase
    {
        private readonly ILearnerManagmentService _learnerManagmentService = learnerManagmentService;


        //[HttpPost("[action]")]
        //public async Task<ApiResponse> UploadLearners(IFormFile formFile)
        //{
        //    if (formFile == null)
        //    {
        //        return new(400, ResponseConstants.BadRequest);
        //    }

        //    return await _storageservice.UploadExcelFile(formFile); 
        //}


        //[HttpGet("[action]")]
        //public async Task<string> DownloadExcel()
        //{
        //    return await _storageservice.DownloadSampleExcelFile();
        //}


        [HttpPost("[action]")]
        public async Task<ApiResponse> AddLearner(AddLearnerModel model)
        {
            return await _learnerManagmentService.AddLearner(model);
        }
    }
}
