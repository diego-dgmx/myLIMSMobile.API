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
    }
}