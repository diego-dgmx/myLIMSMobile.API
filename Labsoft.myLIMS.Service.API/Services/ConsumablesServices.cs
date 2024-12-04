using System.Text;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class ConsumablesServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IConsumablesServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<Consumable>, ErrorResponse>> GetConsumablesByConsumableTypeId(int consumableTypeId)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

            var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Consumables/GetByConsumableTypeIdForMovement?consumableTypeId={consumableTypeId}");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<Consumable>>(json);

            ExternalResponse<List<Consumable>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<Consumable>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    result = new ExternalResponse<List<Consumable>, ErrorResponse>
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
                result = new ExternalResponse<List<Consumable>, ErrorResponse>
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