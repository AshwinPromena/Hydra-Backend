using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class ContactSupportModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobileNumber")]
        public string MobileNumber { get; set; }

    }
}
