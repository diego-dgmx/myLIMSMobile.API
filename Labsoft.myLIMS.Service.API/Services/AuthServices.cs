using System.Net;
using System.Text;
using Entities;
using LabsoftAPI;
using LabsoftAPI.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class AuthServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IAuthServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<LoginResponse, ErrorResponse>> Login(string? email, string? password, string? refreshToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _settings.LabsoftAuthURL)
                {
                    Content = refreshToken != null ? new FormUrlEncodedContent([
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("client_id", _settings.LabsoftAuthClientId),
                        new KeyValuePair<string, string>("refresh_token", refreshToken)
                    ]) : new FormUrlEncodedContent([
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("client_id", _settings.LabsoftAuthClientId),
                        new KeyValuePair<string, string>("audience", _settings.LabsoftAuthAudience),
                        new KeyValuePair<string, string>("username", email ?? ""),
                        new KeyValuePair<string, string>("password", password ?? ""),
                        new KeyValuePair<string, string>("scope", _settings.LabsoftAuthScope)
                    ])
                };

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<LoginResponse, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = JsonConvert.DeserializeObject<LoginResponse>(json)
                    };
                }
                else
                {
                    return new ExternalResponse<LoginResponse, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = JsonConvert.DeserializeObject<ErrorResponse>(json),
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<LoginResponse, ErrorResponse>
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error",
                        Exception = ex
                    }
                };
            }
        }

        public async Task<ExternalResponse<List<MeResponse>, ErrorResponse>> Me(string email)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var response = await _httpClient.GetAsync(
                    $"{_settings.MyLIMSApiURLBase}/v2/Users?$filter=Email eq '{email}'");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<MeResponse>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<MeResponse>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<MeResponse>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error",
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<List<MeResponse>, ErrorResponse>
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error",
                        Exception = ex
                    }
                };
            }
        }

        public async Task<ExternalResponse<dynamic, ErrorResponse>> LogoutBySessionId(string sessionId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var content = new StringContent(sessionId, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_settings.MyLIMSApiURLBase}/v2/Authentication/LogoutBySessionId", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = myLIMSResponse,
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<dynamic, ErrorResponse>
                {
                    StatusCode = (int) HttpStatusCode.InternalServerError,
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error",
                        Exception = ex
                    }
                };
            }
        }
    }
}