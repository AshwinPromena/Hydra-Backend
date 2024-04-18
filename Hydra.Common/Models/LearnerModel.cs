using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class LearnerModel
    {

    }

    public class AddLearnerModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class ExistingLearnerModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
