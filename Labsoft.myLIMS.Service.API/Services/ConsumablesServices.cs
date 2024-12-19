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

        public async Task<ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>> GetConsumables(
            string? identityCenterToken, int? top = null, int? skip = null, string? orderBy = null)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            
            var uriBuilder = new UriBuilder($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            if(top != null)
            {
                query["$top"] = $"{top}";
            }

            if(skip != null)
            {
                query["$skip"] = $"{skip}";
            }

            if(orderBy != null)
            {
                query["$orderby"] = $"{orderBy}";
            }

            uriBuilder.Query = query.ToString();

            var response = await _httpClient.GetAsync(uriBuilder.ToString());

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<ConsumableBasic>>(json);

            ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>
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
                result = new ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>> GetConsumablesByConsumableTypeId(int consumableTypeId)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

            var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Consumables/GetByConsumableTypeIdForMovement?consumableTypeId={consumableTypeId}");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<SimpleConsumableBasic>>(json);

            ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    result = new ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>
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
                result = new ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>
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