using Newtonsoft.Json;

namespace LabsoftAPI
{
    public class ErrorResponse {
        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("error_description")]
        public string? ErrorDescription { get; set; }

        public Exception? Exception { get; set; }
    }
}