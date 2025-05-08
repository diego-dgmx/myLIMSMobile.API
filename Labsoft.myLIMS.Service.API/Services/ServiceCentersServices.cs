using System.Net;
using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class ServiceCentersServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IServiceCentersServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<ServiceCenterBasic>, ErrorResponse>> GetServiceCenters()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

                var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/ServiceCenters?$top=1000");

                var json = await response.Content.ReadAsStringAsync();
            
                if(response.IsSuccessStatusCode)
                {
                    var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<ServiceCenterBasic>>(json);
                    return new ExternalResponse<List<ServiceCenterBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<ServiceCenterBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ServiceCenterBasic>, ErrorResponse>
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