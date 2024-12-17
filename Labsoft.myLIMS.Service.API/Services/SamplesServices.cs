using System.Text;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
                $"{_settings.MyLIMSApiURLBase}/v2/Methods/GetAllWithServiceCenter?$filter=Active eq true");

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

        public async Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetAvailableQCTestsByRoutineBatchId(int routineBatchId)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

            var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/v2/QCTests/GetAvailableByQCRoutineBatchId");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["qCRoutineBatchId"] = routineBatchId.ToString();

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();

            ExternalResponse<List<QCTest>, ErrorResponse> result;

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var qctests = JsonConvert.DeserializeObject<List<QCTest>>(json);

                    result = new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int)response.StatusCode,
                        Success = qctests 
                    };
                }
                else
                {
                    result = new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int)response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error"
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = new ExternalResponse<List<QCTest>, ErrorResponse>
                {
                    Error = new ErrorResponse
                    {
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }


        public async Task<ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>> GetAllSamples(int? sampleType = null, int? top = null, int? skip = null)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
            
            var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/v2/Samples/GetAllMethodsForPerformTask");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["$top"] = top == null ? (await TotalCountSamples()).ToString() : $"{top}";
            if(skip != null)
            {
                query["$skip"] = $"{skip}";
            }

            query["$inlinecount"] = "allpages";
            if(sampleType != null)
            {
                query["$filter"] += $"CurrentStatus/MethodStatus/MethodStatusBehaviorId eq {sampleType}";
            }

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<AnalysisSample>>(json);

            ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
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
                result = new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<AnalysisSample, ErrorResponse>> ValidateSampleCode(int barCode)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
            var response = await _httpClient.GetAsync(
                $"{_settings.MyLIMSApiURLBase}/v2/Samples/GetAllMethodsForPerformTaskByBarCode?barCode={barCode}");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<AnalysisSample>>(json);

            ExternalResponse<AnalysisSample, ErrorResponse> result;

            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<AnalysisSample, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = (myLIMSResponse?.Result ?? []).IsNullOrEmpty() ? null : myLIMSResponse?.Result?[0]
                    };
                }
                else
                {
                    result = new ExternalResponse<AnalysisSample, ErrorResponse>
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
                result = new ExternalResponse<AnalysisSample, ErrorResponse>
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
            var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/v2/Samples/GetAllMethodsForPerformTask");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["$top"] = "0";
            query["$inlinecount"] = "allpages";
            if(sampleType != null)
            {
                query["$filter"] += $"CurrentStatus/MethodStatus/MethodStatusBehaviorId eq {sampleType}";
            }

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<dynamic>>(json);

            return myLIMSResponse?.TotalCount ?? 0;
        }

        public async Task<ExternalResponse<List<TaskForPerform>, ErrorResponse>> GetForPerformTask(int[] sampleMethodIds)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

            var queryParams = sampleMethodIds
                .Select((value, index) => $"sampleMethodIds[{index}]={value}")
                .ToArray();

            var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Samples/Methods/GetForPerformTask?{string.Join("&", queryParams)}");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<List<TaskForPerform>>(json);

            ExternalResponse<List<TaskForPerform>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<TaskForPerform>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<List<TaskForPerform>, ErrorResponse>
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
                result = new ExternalResponse<List<TaskForPerform>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<string, ErrorResponse>> PerformTask(bool calculate, PerformTaskDTO body)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

            var content = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8, "application/json"
            );

            var response = await _httpClient.PostAsync(
                $"{_settings.MyLIMSApiURLBase}/v2/Samples/PerformTask?calculate={calculate}", content);

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<string>(json);

            ExternalResponse<string, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<string, ErrorResponse>
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
                result = new ExternalResponse<string, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<string, ErrorResponse>> AdvanceStep(PerformTaskBackgroundParamsDTO body)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

            var content = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8, "application/json"
            );

            var response = await _httpClient.PostAsync(
                $"{_settings.MyLIMSApiURLBase}/v2/SampleMethods/AdvanceStep", content);

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<string>(json);

            ExternalResponse<string, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<string, ErrorResponse>
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
                result = new ExternalResponse<string, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<SampleMethodsCount, ErrorResponse>> GetSampleMethodsCount(string? identityCenterToken)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);

            var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/SampleMethods/SampleMethodsCount");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<SampleMethodsCount>(json);

            ExternalResponse<SampleMethodsCount, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<SampleMethodsCount, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<SampleMethodsCount, ErrorResponse>
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
                result = new ExternalResponse<SampleMethodsCount, ErrorResponse>
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