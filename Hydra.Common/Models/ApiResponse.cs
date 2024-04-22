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

    public class PagedResponseInput
    {
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 10;

        [JsonProperty("searchString")]
        public string SearchString { get; set; } = "";
    }

    public class PagedResponse<T> : PagedResponseInput
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonProperty("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }

        [JsonProperty("totalRecords")]
        public long TotalRecords { get; set; }

        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class PagedResponseOutput<T>
    {
        [JsonProperty("totalCount")]
        public long TotalCount { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
