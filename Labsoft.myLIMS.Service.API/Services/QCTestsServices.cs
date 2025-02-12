using System.Net;
using System.Text;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class QCTestsServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IQCTestsServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<int>, ErrorResponse>> AttachSampleMethodsToQCTest(AttachSampleMethodsToQCTestDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                
                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync(
                    $"{_settings.MyLIMSApiURLBase}/v2/QCTests/AttachSampleMethodsToQCTest", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<List<int>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<int>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<int>, ErrorResponse>
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
                return new ExternalResponse<List<int>, ErrorResponse>
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

        public async Task<ExternalResponse<int, ErrorResponse>> CreateNewQCTest(CreateNewQCTestDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                
                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync(
                    $"{_settings.MyLIMSApiURLBase}/v2/QCTests/CreateNewQCTest", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<int>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<int, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<int, ErrorResponse>
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
                return new ExternalResponse<int, ErrorResponse>
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

        public async Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetAvailableByQCRoutineBatchId(int id, string? identityCenterToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                
                var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/v1/QCTests/GetAvailableByQCRoutineBatchId");

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["id"] = id.ToString();

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<List<QCTest>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<QCTest>, ErrorResponse>
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
                return new ExternalResponse<List<QCTest>, ErrorResponse>
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

        public async Task<ExternalResponse<List<QCTestLink>, ErrorResponse>> GetControlSamplesByQCTestId(int id, string? identityCenterToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                
                var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/v1/QCTests/GetControlSamplesByQCTestId");

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["id"] = id.ToString();

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<List<QCTestLink>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<QCTestLink>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<QCTestLink>, ErrorResponse>
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
                return new ExternalResponse<List<QCTestLink>, ErrorResponse>
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

        public async Task<ExternalResponse<List<QCTestLink>, ErrorResponse>> GetLinkedSamplesByQCTestId(int id, string? identityCenterToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                
                var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/v1/QCTests/GetLinkedSamplesByQCTestId");

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["id"] = id.ToString();

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<List<QCTestLink>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<QCTestLink>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<QCTestLink>, ErrorResponse>
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
                return new ExternalResponse<List<QCTestLink>, ErrorResponse>
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

        public async Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetQCTests(string? identityCenterToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                
                var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/v1/QCTests");

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["$top"] = (await TotalCountQCTests()).ToString();

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<QCTest>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<QCTest>, ErrorResponse>
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
                return new ExternalResponse<List<QCTest>, ErrorResponse>
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

        private async Task<int> TotalCountQCTests() {
            var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/v1/QCTests");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["$top"] = "0";

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<dynamic>>(json);

            return myLIMSResponse?.TotalCount ?? 0;
        }
    }
}