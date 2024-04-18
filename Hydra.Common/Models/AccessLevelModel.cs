using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class AccessLevelModel
    {
        [JsonProperty("accessLevelID")]
        public long AccessLevelId { get; set; }

        [JsonProperty("accessLevelName")]
        public string AccessLevelName { get; set; }
    }
}
