using Newtonsoft.Json;


public class LoginResponse<TSuccess, TError> {
    public TSuccess? Success { get; set; }
    public TError? Error { get; set; }
}
public class LoginSuccessResponse {
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("expires_in")]
    public int? ExpiresIn { get; set; }

    [JsonProperty("token_type")]
    public string? TokenType { get; set; }

    [JsonProperty("scope")]
    public string? Scope { get; set; }
}

public class LoginErrorResponse {
    [JsonProperty("error")]
    public string? Error { get; set; }

    [JsonProperty("error_description")]
    public string? ErrorDescription { get; set; }
}