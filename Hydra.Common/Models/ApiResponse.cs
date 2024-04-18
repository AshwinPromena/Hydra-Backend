using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class ApiResponse
    {
        public ApiResponse() { }
        public ApiResponse(long statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class ServiceResponse<T> : ApiResponse
    {
        public ServiceResponse() { }
        public ServiceResponse(long statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public ServiceResponse(long statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
