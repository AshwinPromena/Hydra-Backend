using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class Base64String
    {
        [JsonProperty("filrBase64String")]
        public string FileBase64String { get; set; }
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

    public class AssignBadgeToLearnerModel
    {
        [JsonProperty("userIds")]
        public List<long> UserIds { get; set; }

        [JsonProperty("badgeIds")]
        public List<long> BadgeIds { get; set; }
    }

    public class LearnerDashBoardModel
    {
        [JsonProperty("learnerInTotal")]
        public int LearnerInTotal { get; set; }

        [JsonProperty("learnerWithBadge")]
        public int LearnerWithBadge { get; set; }

        [JsonProperty("learnerWithoutBadge")]
        public int LearnerWithoutBadge { get; set; }
    }

    public class GetLearnerModel
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

    }
}
