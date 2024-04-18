using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class DepartmentModel
    {
        [JsonProperty("id")]
        public long DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
    }
}
