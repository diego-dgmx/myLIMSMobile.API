using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class AuthServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IAuthServices
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _settings = settings.Value;

    public async Task<LoginResponse<LoginSuccessResponse, LoginErrorResponse>> Login(string email, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _settings.LabsoftAuthURL)
        {
            Content = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _settings.LabsoftAuthClientId),
                new KeyValuePair<string, string>("username", email),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("scope", _settings.LabsoftAuthScope)
            ])
        };

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        LoginResponse<LoginSuccessResponse, LoginErrorResponse> result;

        
        try
        {
            if(response.IsSuccessStatusCode)
            {
                result = new LoginResponse<LoginSuccessResponse, LoginErrorResponse>
                {
                    Success = JsonConvert.DeserializeObject<LoginSuccessResponse>(json)
                };
            }
            else
            {
                result = new LoginResponse<LoginSuccessResponse, LoginErrorResponse>
                {
                    Error = JsonConvert.DeserializeObject<LoginErrorResponse>(json)
                };
            }
        }
        catch(Exception) {
            result = new LoginResponse<LoginSuccessResponse, LoginErrorResponse>
            {
                Error = new LoginErrorResponse{
                    ErrorDescription = "Unknown error, please try again"
                }
            };
        }

        return result;
    }
}