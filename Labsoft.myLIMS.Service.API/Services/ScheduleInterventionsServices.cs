using System.Net;
using System.Text;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Services {
    public class ScheduleInterventionsServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IScheduleInterventionsServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<ScheduleAnalysisGroupBasic>, ErrorResponse>> GetScheduleAnalysisGroups(string? identityCenterToken, string? identityCompany, int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/ScheduleIntervention/{id}/AnalysisGroup");

                var json = await response.Content.ReadAsStringAsync();
            
                if(response.IsSuccessStatusCode)
                {
                    var myLIMSResponse = JsonConvert.DeserializeObject<List<ScheduleAnalysisGroupBasic>>(json);
                    return new ExternalResponse<List<ScheduleAnalysisGroupBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<ScheduleAnalysisGroupBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ScheduleAnalysisGroupBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<ScheduleControlPlanBasic, ErrorResponse>> GetScheduleControlPlan(string? identityCenterToken, string? identityCompany, int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/ScheduleIntervention/{id}/ControlPlans");

                var json = await response.Content.ReadAsStringAsync();
            
                if(response.IsSuccessStatusCode)
                {
                    var myLIMSResponse = JsonConvert.DeserializeObject<ScheduleControlPlanBasic>(json);
                    return new ExternalResponse<ScheduleControlPlanBasic, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<ScheduleControlPlanBasic, ErrorResponse>
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
                return new ExternalResponse<ScheduleControlPlanBasic, ErrorResponse>
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

        public async Task<ExternalResponse<MyLIMSResponseBase<ScheduleInterventionBasic>, ErrorResponse>> GetScheduleInterventions(
            string? sortParam = null, string? filter = null, int? top = null, int? skip = null)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/v2/ScheduleIntervention");

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["$top"] = top == null ? "10" : $"{top}";

                if(skip != null)
                {
                    query["$skip"] = $"{skip}";
                }

                query["$inlinecount"] = "allpages";

                if(!filter.IsNullOrEmpty())
                {
                    query["$filter"] = filter;
                }

                if(!sortParam.IsNullOrEmpty())
                {
                    query["$orderby"] = sortParam;
                }

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<ScheduleInterventionBasic>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<MyLIMSResponseBase<ScheduleInterventionBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<MyLIMSResponseBase<ScheduleInterventionBasic>, ErrorResponse>
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
                return new ExternalResponse<MyLIMSResponseBase<ScheduleInterventionBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<List<ScheduleSampleInterventionBasic>, ErrorResponse>> GetScheduleSampleInterventions(string? identityCenterToken, string? identityCompany, int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/ScheduleIntervention/{id}/Interventions");

                var json = await response.Content.ReadAsStringAsync();
            
                if(response.IsSuccessStatusCode)
                {
                    var myLIMSResponse = JsonConvert.DeserializeObject<List<ScheduleSampleInterventionBasic>>(json);
                    return new ExternalResponse<List<ScheduleSampleInterventionBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<ScheduleSampleInterventionBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ScheduleSampleInterventionBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<List<ScheduleSpecificationBasic>, ErrorResponse>> GetScheduleSpecifications(string? identityCenterToken, string? identityCompany, int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/ScheduleIntervention/{id}/Specifications");

                var json = await response.Content.ReadAsStringAsync();
            
                if(response.IsSuccessStatusCode)
                {
                    var myLIMSResponse = JsonConvert.DeserializeObject<List<ScheduleSpecificationBasic>>(json);
                    return new ExternalResponse<List<ScheduleSpecificationBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<ScheduleSpecificationBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ScheduleSpecificationBasic>, ErrorResponse>
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