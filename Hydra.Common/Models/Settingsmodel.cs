﻿using Newtonsoft.Json;

namespace Hydra.Common.Models
{
    public class GetAllDeletedUserInputModel : PagedResponseInput
    {
        [JsonProperty("type")]
        public int Type { get; set; }
    }

    public class GetAllDeletedUserModel
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("deletedUserId")]
        public long DeletedUserId { get; set; }

        [JsonProperty("deletedUserName")]
        public string DeletedUserName { get; set; }

        [JsonProperty("deletedUserEmail")]
        public string DeletedUserEmail { get; set; }

        [JsonProperty("deletedDate")]
        public DateTime DeletdDate { get; set; }
    }
}