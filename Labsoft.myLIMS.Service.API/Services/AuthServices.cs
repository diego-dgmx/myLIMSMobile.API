using System.Net.Http.Headers;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class AuthServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IAuthServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<LoginResponse, ErrorResponse>> Login(string email, string password)
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
            ExternalResponse<LoginResponse, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<LoginResponse, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = JsonConvert.DeserializeObject<LoginResponse>(json)
                    };
                }
                else
                {
                    result = new ExternalResponse<LoginResponse, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = JsonConvert.DeserializeObject<ErrorResponse>(json)
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<LoginResponse, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<MeResponse, ErrorResponse>> Me(string authToken, string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.GetAsync(
                $"{_settings.LabsoftIdentityCenterApiURLBase}/Users/email/{email}");

            var json = await response.Content.ReadAsStringAsync();
            ExternalResponse<MeResponse, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<MeResponse, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = JsonConvert.DeserializeObject<MeResponse>(json)
                    };
                }
                else
                {
                    result = new ExternalResponse<MeResponse, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = JsonConvert.DeserializeObject<ErrorResponse>(json)
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<MeResponse, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }
    }
}