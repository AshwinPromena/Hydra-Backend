using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class DepartmentDropDownModel
    {
        [JsonProperty("departmentId")]
        public long DepartmentId { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }
    }

    public class AccessLevelDropDownModel
    {
        [JsonProperty("accessLevelId")]
        public long AccessLevelId { get; set; }

        [JsonProperty("accessLevelName")]
        public string AccessLevelName { get; set; }
    }

    public class UserDropDownModel
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }
    }

    public class BadgeDropDownModel
    {
        [JsonProperty("badgeId")]
        public long BadgeId { get; set; }

        [JsonProperty("badgeName")]
        public string BadgeName { get; set; }
    }

    public class BadgeSortByDropDownModel
    {
        [JsonProperty("sortId")]
        public long SortId { get; set; }

        [JsonProperty("sortName")]
        public string SortName { get; set; }
    }

    public class StaffSortByDropDownModel
    {
        [JsonProperty("sortId")]
        public long SortId { get; set; }

        [JsonProperty("sortName")]
        public string SortName { get; set; }
    }

    public class DeletedUserDropDownModel
    {
        [JsonProperty("typeId")]
        public long TypeId { get; set; }

        [JsonProperty("typeName")]
        public string TypeName  { get; set; }
    }

    public class BadgeTypeDropDownModel
    {
        [JsonProperty("badgeTypeId")]
        public long BadgeTypeId { get; set; }

        [JsonProperty("badgeTypeName")]
        public string BadgeTypeName { get; set; }
    }
}
