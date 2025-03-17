using System.Net;
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
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var response = await _httpClient.GetAsync(
                    $"{_settings.MyLIMSApiURLBase}/v2/Methods/GetAllWithServiceCenter?$filter=Active eq true");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<AnalysisMethod>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<AnalysisMethod>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<AnalysisMethod>, ErrorResponse>
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
                return new ExternalResponse<List<AnalysisMethod>, ErrorResponse>
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

        public async Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetAvailableQCTestsByRoutineBatchId(int routineBatchId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

                var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/v2/QCTests/GetAvailableByQCRoutineBatchId");
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["qCRoutineBatchId"] = routineBatchId.ToString();

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var qctests = JsonConvert.DeserializeObject<List<QCTest>>(json);

                    return new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int)response.StatusCode,
                        Success = qctests 
                    };
                }
                else
                {
                    return new ExternalResponse<List<QCTest>, ErrorResponse>
                    {
                        StatusCode = (int)response.StatusCode,
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
            catch (Exception ex)
            {
                return new ExternalResponse<List<QCTest>, ErrorResponse>
                {
                    Error = new ErrorResponse
                    {
                        Error = "unknown_error",
                        ErrorDescription = "exception_error",
                        Exception = ex
                    }
                };
            }
        }


        public async Task<ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>> GetAllSamples(
            int? sampleType = null, string? sortParam = null, string? filter = null, int? top = null, int? skip = null)
        {
            try
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
                    query["$filter"] = $"CurrentStatus/MethodStatus/MethodStatusBehaviorId eq {sampleType}";
                }

                if(!filter.IsNullOrEmpty())
                {
                    query["$filter"] += $" and {filter}";
                }

                if(!sortParam.IsNullOrEmpty())
                {
                    query["$orderby"] = sortParam;
                }

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<AnalysisSample>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
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
                return new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
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

        public async Task<ExternalResponse<AnalysisSample, ErrorResponse>> ValidateSampleCode(int barCode)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var response = await _httpClient.GetAsync(
                    $"{_settings.MyLIMSApiURLBase}/v2/Samples/GetAllMethodsForPerformTaskByBarCode?barCode={barCode}");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<AnalysisSample>>(json);

                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<AnalysisSample, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = (myLIMSResponse?.Result ?? []).IsNullOrEmpty() ? null : myLIMSResponse?.Result?[0]
                    };
                }
                else
                {
                    return new ExternalResponse<AnalysisSample, ErrorResponse>
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
                return new ExternalResponse<AnalysisSample, ErrorResponse>
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
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

                var queryParams = sampleMethodIds
                    .Select((value, index) => $"sampleMethodIds[{index}]={value}")
                    .ToArray();

                var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Samples/Methods/GetForPerformTask?{string.Join("&", queryParams)}");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<List<TaskForPerform>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<TaskForPerform>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<TaskForPerform>, ErrorResponse>
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
                return new ExternalResponse<List<TaskForPerform>, ErrorResponse>
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

        public async Task<ExternalResponse<string, ErrorResponse>> PerformTask(bool calculate, PerformTaskDTO body)
        {
            try
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
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<string, ErrorResponse>
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
                return new ExternalResponse<string, ErrorResponse>
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

        public async Task<ExternalResponse<string, ErrorResponse>> AdvanceStatus(int sampleMethodId, AdvanceMethodStatusParamsDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync(
                    $"{_settings.MyLIMSApiURLBase}/v2/Samples/Methods/{sampleMethodId}/AdvanceStatus", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<string>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<string, ErrorResponse>
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
                return new ExternalResponse<string, ErrorResponse>
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

        public async Task<ExternalResponse<SampleMethodsCount, ErrorResponse>> GetSampleMethodsCount(string? identityCenterToken, string? methodMasterIds = null, string? sampleTypeIds = null, string? methodStatusIds = null, string? serviceAreaIds = null, string? sampleReasonIds = null, string? workIds = null, string? startUserIds = null, string? collectPointIds = null, string? qcTestIds = null, string? sampleIds = null, string? sampleIdentification = null, string? sampleControlNumber = null, string? validityStartDateTime = null, string? validityEndDateTime = null, string? executionStartDateTime = null, string? executionEndDateTime = null, string? conclusionStartDateTime = null, string? conclusionEndDateTime = null, string? receivedStartDateTime = null, string? receivedEndDateTime = null, string? startStartDateTime = null, string? startEndDateTime = null, string? takenStartDateTime = null, string? takenEndDateTime = null)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/v1/SampleMethods/SampleMethodsCount");

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                if(!methodMasterIds.IsNullOrEmpty()) {
                    query["MethodMasterIds"] = methodMasterIds;
                }
                if(!sampleTypeIds.IsNullOrEmpty()) {
                    query["SampleTypeIds"] = sampleTypeIds;
                }
                if(!methodStatusIds.IsNullOrEmpty()) {
                    query["MethodStatusIds"] = methodStatusIds;
                }
                if(!serviceAreaIds.IsNullOrEmpty()) {
                    query["ServiceAreaIds"] = serviceAreaIds;
                }
                if(!sampleReasonIds.IsNullOrEmpty()) {
                    query["SampleReasonIds"] = sampleReasonIds;
                }
                if(!workIds.IsNullOrEmpty()) {
                    query["WorkIds"] = workIds;
                }
                if(!startUserIds.IsNullOrEmpty()) {
                    query["StartUserIds"] = startUserIds;
                }
                if(!collectPointIds.IsNullOrEmpty()) {
                    query["CollectPointIds"] = collectPointIds;
                }
                if(!qcTestIds.IsNullOrEmpty()) {
                    query["QcTestIds"] = qcTestIds;
                }
                if(!sampleIds.IsNullOrEmpty()) {
                    query["SampleIds"] = sampleIds;
                }
                if(!sampleIdentification.IsNullOrEmpty()) {
                    query["SampleIdentification"] = sampleIdentification;
                }
                if(!sampleControlNumber.IsNullOrEmpty()) {
                    query["SampleControlNumber"] = sampleControlNumber;
                }
                if(!validityStartDateTime.IsNullOrEmpty()) {
                    query["ValidityStartDateTime"] = validityStartDateTime;
                }
                if(!validityEndDateTime.IsNullOrEmpty()) {
                    query["ValidityEndDateTime"] = validityEndDateTime;
                }
                if(!executionStartDateTime.IsNullOrEmpty()) {
                    query["ExecutionStartDateTime"] = executionStartDateTime;
                }
                if(!executionEndDateTime.IsNullOrEmpty()) {
                    query["ExecutionEndDateTime"] = executionEndDateTime;
                }
                if(!conclusionStartDateTime.IsNullOrEmpty()) {
                    query["ConclusionStartDateTime"] = conclusionStartDateTime;
                }
                if(!conclusionEndDateTime.IsNullOrEmpty()) {
                    query["ConclusionEndDateTime"] = conclusionEndDateTime;
                }
                if(!receivedStartDateTime.IsNullOrEmpty()) {
                    query["ReceivedStartDateTime"] = receivedStartDateTime;
                }
                if(!receivedEndDateTime.IsNullOrEmpty()) {
                    query["ReceivedEndDateTime"] = receivedEndDateTime;
                }
                if(!startStartDateTime.IsNullOrEmpty()) {
                    query["StartStartDateTime"] = startStartDateTime;
                }
                if(!startEndDateTime.IsNullOrEmpty()) {
                    query["StartEndDateTime"] = startEndDateTime;
                }
                if(!takenStartDateTime.IsNullOrEmpty()) {
                    query["TakenStartDateTime"] = takenStartDateTime;
                }
                if(!takenEndDateTime.IsNullOrEmpty()) {
                    query["TakenEndDateTime"] = takenEndDateTime;
                }

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<SampleMethodsCount>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<SampleMethodsCount, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<SampleMethodsCount, ErrorResponse>
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
                return new ExternalResponse<SampleMethodsCount, ErrorResponse>
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

        public async Task<ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>> GetForExecutionSamples(
            string? identityCenterToken, int? sampleType = null, string? sortParam = null, string? filter = null, int? top = null, int? skip = null)
        {
            try {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/v1/SampleMethods/ForExecution");

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["$top"] = top == null ? (await TotalCountSamples()).ToString() : $"{top}";
                if(skip != null)
                {
                    query["$skip"] = $"{skip}";
                }

                query["$count"] = "true";
                if(sampleType != null)
                {
                    query["$filter"] = $"CurrentStatus/MethodStatus/MethodStatusBehaviorId eq {sampleType}";
                }

                if(!filter.IsNullOrEmpty())
                {
                    query["$filter"] += $" and {filter}";
                }

                if(!sortParam.IsNullOrEmpty())
                {
                    query["$orderby"] = sortParam;
                }

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<AnalysisSample>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
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
                return new ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>
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

        public async Task<ExternalResponse<SampleMethodFilterOptions, ErrorResponse>> GetFilterOptions(string? identityCenterToken)
        {
            try {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/SampleMethods/SampleMethodFilters");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<SampleMethodFilterOptions>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<SampleMethodFilterOptions, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<SampleMethodFilterOptions, ErrorResponse>
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
                return new ExternalResponse<SampleMethodFilterOptions, ErrorResponse>
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