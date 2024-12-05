using System.Text;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class MeasurementUnitsServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IMeasurementUnitsServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<MeasurementUnitBasic>, ErrorResponse>> GetMeasurementUnits()
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

            var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/MeasurementUnits?$top=1000");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<MeasurementUnitBasic>>(json);

            ExternalResponse<List<MeasurementUnitBasic>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<MeasurementUnitBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    result = new ExternalResponse<List<MeasurementUnitBasic>, ErrorResponse>
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
                result = new ExternalResponse<List<MeasurementUnitBasic>, ErrorResponse>
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