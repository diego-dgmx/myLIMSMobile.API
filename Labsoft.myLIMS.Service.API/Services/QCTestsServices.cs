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

        public async Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetAvailableByQCRoutineBatchId(int id, string? identityCenterToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            
            var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/QCTests/GetAvailableByQCRoutineBatchId");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["id"] = id.ToString();

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<List<QCTest>>(json);

            ExternalResponse<List<QCTest>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error"
                        }
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<List<QCTest>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<List<QCTestLink>, ErrorResponse>> GetControlSamplesByQCTestId(int id, string? identityCenterToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            
            var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/QCTests/GetControlSamplesByQCTestId");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["id"] = id.ToString();

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<List<QCTestLink>>(json);

            ExternalResponse<List<QCTestLink>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<QCTestLink>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<List<QCTestLink>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error"
                        }
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<List<QCTestLink>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<List<QCTestLink>, ErrorResponse>> GetLinkedSamplesByQCTestId(int id, string? identityCenterToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            
            var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/QCTests/GetLinkedSamplesByQCTestId");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["id"] = id.ToString();

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<List<QCTestLink>>(json);

            ExternalResponse<List<QCTestLink>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<QCTestLink>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<List<QCTestLink>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error"
                        }
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<List<QCTestLink>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetQCTests(string? identityCenterToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            
            var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/QCTests");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["$top"] = (await TotalCountQCTests()).ToString();

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<QCTest>>(json);

            ExternalResponse<List<QCTest>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    result = new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error"
                        }
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<List<QCTest>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        private async Task<int> TotalCountQCTests() {
            var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/QCTests");

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