using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class AddDepartmentModel
    {
        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
    }


    public class AddUniversityModel
    {
        [JsonProperty("universityName")]
        public string UniversityName { get; set; }

        [JsonProperty("universityLogo")]
        public string UniversityLogo { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }


    public class GetUniversityByIdModel : AddUniversityModel
    {
        [JsonProperty("id")]
        public long UniversityId { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }


    public class GetAllUniversityModel : PagedResponseInput
    {
        [JsonProperty("universityName")]
        public string UniversityName { get; set; }

        [JsonProperty("universityLogo")]
        public string UniversityLogo { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("universityId")]
        public long UniversityId { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }


    public class UpdateUniversityModel
    {
        [JsonProperty("id")]
        public long UniversityId { get; set; }

        [JsonProperty("universityName")]
        public string UniversityName { get; set; }

        [JsonProperty("universityLogo")]
        public string UniversityLogo { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
