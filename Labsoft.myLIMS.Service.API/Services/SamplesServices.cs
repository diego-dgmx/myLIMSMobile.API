using System.Net.Http.Headers;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class SamplesServices(HttpClient httpClient, IOptions<ApiSettings> settings) : ISamplesServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<AnalysisMethod>, ErrorResponse>> GetAllMethods()
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
            var response = await _httpClient.GetAsync(
                $"{_settings.MyLIMSApiURLBase}/Methods/GetAllWithServiceCenter?$filter=Active eq true");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<AnalysisMethod>>(json);

            ExternalResponse<List<AnalysisMethod>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<AnalysisMethod>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    result = new ExternalResponse<List<AnalysisMethod>, ErrorResponse>
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
                result = new ExternalResponse<List<AnalysisMethod>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<List<AnalysisSample>, ErrorResponse>> GetAllSamples(int? sampleType = null)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
            
            var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/Samples/GetAllMethodsForPerformTask");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["$top"] = (await TotalCountSamples()).ToString();
            query["$inlinecount"] = "allpages";
            if(sampleType != null)
            {
                query["$filter"] = $"CurrentStatus/MethodStatus/MethodStatusBehaviorId eq {sampleType}";
            }

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<AnalysisSample>>(json);

            ExternalResponse<List<AnalysisSample>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<AnalysisSample>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    result = new ExternalResponse<List<AnalysisSample>, ErrorResponse>
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
                result = new ExternalResponse<List<AnalysisSample>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        private async Task<int> TotalCountSamples(int? sampleType = null) {
            var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/Samples/GetAllMethodsForPerformTask");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["$top"] = "0";
            query["$inlinecount"] = "allpages";
            if(sampleType != null)
            {
                query["$filter"] = $"CurrentStatus/MethodStatus/MethodStatusBehaviorId eq {sampleType}";
            }

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<dynamic>>(json);

            return myLIMSResponse?.TotalCount ?? 0;
        }
    }
}