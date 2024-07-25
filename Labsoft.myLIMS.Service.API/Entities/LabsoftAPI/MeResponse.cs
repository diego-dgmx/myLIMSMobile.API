using Newtonsoft.Json;

namespace LabsoftAPI
{
    public class MeResponse {
        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("emailConfirmed")]
        public bool? EmailConfirmed { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("phoneNumberConfirmed")]
        public bool? PhoneNumberConfirmed { get; set; }

        [JsonProperty("lockoutEnabled")]
        public bool? LockoutEnabled { get; set; }

        [JsonProperty("twoFactorEnabled")]
        public bool? TwoFactorEnabled { get; set; }

        [JsonProperty("accessFailedCount")]
        public int? AccessFailedCount { get; set; }

        [JsonProperty("lockoutEnd")]
        public string? LockoutEnd { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }
    }
}