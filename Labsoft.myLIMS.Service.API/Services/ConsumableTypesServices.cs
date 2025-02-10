using System.Net;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class ConsumableTypesServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IConsumableTypesServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<ConsumableTypeBasic>, ErrorResponse>> GetConsumableTypes(string? identityCenterToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/ConsumableTypes?$top=1000");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<ConsumableTypeBasic>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<ConsumableTypeBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result ?? []
                    };
                }
                else
                {
                    return new ExternalResponse<List<ConsumableTypeBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ConsumableTypeBasic>, ErrorResponse>
                {
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