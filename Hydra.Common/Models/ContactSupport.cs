using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class ContactSupportModel
    {
        [JsonProperty("name")]
        [JsonRequired]
        public string Name { get; set; }

        [JsonProperty("email")]
        [JsonRequired]
        public string Email { get; set; }

        [JsonProperty("mobileNumber")]
        [JsonRequired]
        public string MobileNumber { get; set; }

        [JsonProperty("description")]
        [JsonRequired]
        public string Description { get; set; }
    }
}
