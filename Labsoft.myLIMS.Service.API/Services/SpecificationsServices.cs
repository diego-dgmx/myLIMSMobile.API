using System.Net;
using System.Web;
using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class SpecificationsServices(HttpClient httpClient, IOptions<ApiSettings> settings) : ISpecificationsServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<SpecificationBasic>, ErrorResponse>> GetSpecifications()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/v2/Specifications");
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["$inlinecount"] = "allpages";
                query["$top"] = "1000";
                query["$filter"] = "Active eq true and ControlPlan eq false";

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<SpecificationBasic>>(json);

                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<SpecificationBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<SpecificationBasic>, ErrorResponse>
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
                return new ExternalResponse<List<SpecificationBasic>, ErrorResponse>
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