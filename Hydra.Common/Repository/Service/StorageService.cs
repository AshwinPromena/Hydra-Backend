using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Hydra.Common.Globle;
using Hydra.Common.Models;
using Hydra.Common.Repository.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Hydra.Common.Repository.Service
{
    public class StorageServices(IConfiguration configuration) : FileExtentionService, IStorageService
    {
        private readonly IConfiguration _configuration = configuration;
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;

        private static int MAX_FILE_SIZE = 1024 * 1024 * 70; // 70 MB

        private static string[] EXTENSION_LOWER_CASE = new string[] { "pdf", "jpg", "png", "jpeg", "gif", "mp4", "webp", "txt" };

        private string GenerateS3Path(string path, string fileName)
        {
            var key = $"{path}/{Guid.NewGuid()}{Path.GetExtension(fileName).ToLower()}";
            return key;
        }

        private bool ValidateFilesSize(IFormFile file)
        {
            return file == null ? false : file.Length > MAX_FILE_SIZE ? false : true;
        }

        private bool IsExtensionsAvailable(string extension)
        {
            return EXTENSION_LOWER_CASE.Contains(extension) ? false : true;
        }

        public async Task<ServiceResponse<string>> UploadFile(string path, IFormFile file)
        {
            try
            {
                using IAmazonS3 client = new AmazonS3Client(_configuration.GetConnectionString("AccessKey"), _configuration.GetConnectionString("SecretKey"), bucketRegion);
                if (file == null)
                    return new ServiceResponse<string>(StatusCodes.Status400BadRequest, "File was null", null);

                if (ValidateFilesSize(file) == false)
                    return new ServiceResponse<string>(StatusCodes.Status413RequestEntityTooLarge, "Entity Too Large", null);

                if (IsExtensionsAvailable(file.FileName.Split(".")[1]))
                    return new ServiceResponse<string>(StatusCodes.Status406NotAcceptable, "Not Acceptable", null);

                var filetransferutility = new TransferUtility(client);
                var filePath = GenerateS3Path(path, file.FileName);
                using (var fileStream = file.OpenReadStream())
                {
                    await filetransferutility.UploadAsync(fileStream, _configuration.GetConnectionString("BucketName"), filePath);
                }
                await client.PutACLAsync(new PutACLRequest
                {
                    BucketName = _configuration.GetConnectionString("BucketName"),
                    CannedACL = S3CannedACL.PublicReadWrite,
                    Key = filePath
                });
                return new ServiceResponse<string>(StatusCodes.Status200OK, "", $"{_configuration.GetConnectionString("AWSBasePath")}{filePath}");

            }
            catch (AmazonS3Exception e)
            {
                return new ServiceResponse<string>(StatusCodes.Status500InternalServerError, e.Message, null);
            }
            catch (Exception e)
            {
                return new ServiceResponse<string>(StatusCodes.Status500InternalServerError, e.Message, null);
            }
        }

        public async Task<ServiceResponse<string>> UploadFile(string path, string file)
        {
            if (file.Contains("https://new-hydra.s3.ap-south-1.amazonaws.com/hydra/media/"))
            {
                return new(200, ResponseConstants.Success, file);
            }
            try
            {
                byte[] inBytes = Convert.FromBase64String(file);
                var stream = new MemoryStream(inBytes);
                Random random = new Random();
                var fileName = random.Next(100000, 199999).ToString();

                var accessKey = Environment.GetEnvironmentVariable("AWS:accessKey");
                var secretKey = Environment.GetEnvironmentVariable("AWS:secretKey");
                var bucketName = Environment.GetEnvironmentVariable("AWS:bucketName");
                var regionEndPoint = Environment.GetEnvironmentVariable("AWS:Region");
                var baseUrl = Environment.GetEnvironmentVariable("AWS:base_url");
                //using IAmazonS3 client = new AmazonS3Client(_configuration.GetValue<string>("AWS:accessKey"), _configuration.GetValue<string>("AWS:secretKey"), Amazon.RegionEndpoint.APSouth1);
                using IAmazonS3 client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.APSouth1);
                var filetransferutility = new TransferUtility(client);
                var extension = GetExtension(file[..5]);
                var filePath = GenerateS3Path(path, $"{fileName}{extension}");
                await filetransferutility.UploadAsync(stream, bucketName, filePath);
                await client.PutACLAsync(new PutACLRequest
                {
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = filePath
                });
                return new ServiceResponse<string>(StatusCodes.Status200OK, "", $"{baseUrl}{filePath}");
            }
            catch (AmazonS3Exception e)
            {
                return new ServiceResponse<string>(StatusCodes.Status500InternalServerError, e.Message, null);
            }
            catch (Exception e)
            {
                return new ServiceResponse<string>(StatusCodes.Status500InternalServerError, e.Message, null);
            }
        }

        public async Task<ApiResponse> DeleteFile(string path)
        {
            if (path is null)
                return new ApiResponse(400, "Path cannot be null");
            using IAmazonS3 client = new AmazonS3Client(_configuration.GetConnectionString("AccessKey"), _configuration.GetConnectionString("SecretKey"), bucketRegion);
            DeleteObjectRequest request = new()
            {
                BucketName = _configuration.GetConnectionString("BucketName"),
                Key = path
            };
            try
            {
                var abc = await client.DeleteObjectAsync(request);
            }
            catch (AmazonS3Exception ex)
            {

            }
            return new ApiResponse(200, "Successfully Removed");
        }
    }
}
