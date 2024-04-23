using Hydra.Common.Models;
using Hydra.Common.Repository.Service;
using Microsoft.AspNetCore.Http;

namespace Hydra.Common.Repository.IService
{
    public interface IStorageService
    {
        Task<ServiceResponse<string>> UploadFile(string path, string file);

        Task<ServiceResponse<string>> UploadFile(string path, IFormFile file);

        Task<ApiResponse> DeleteFile(string path);
    }
}
