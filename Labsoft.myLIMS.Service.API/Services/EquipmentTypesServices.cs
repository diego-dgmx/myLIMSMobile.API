using System.Net;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Services {
    public class EquipmentTypesServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IEquipmentTypesServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<EquipmentType>, ErrorResponse>> GetEquipmentTypes()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/EquipmentTypes?$inlinecount=allpages");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<EquipmentType>>(json);

                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<EquipmentType>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<EquipmentType>, ErrorResponse>
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
                return new ExternalResponse<List<EquipmentType>, ErrorResponse>
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