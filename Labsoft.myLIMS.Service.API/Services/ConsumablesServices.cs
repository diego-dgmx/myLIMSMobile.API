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

        public async Task<ExternalResponse<ConsumableBasic, ErrorResponse>> GetConsumable(string? identityCenterToken, int id)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{id}");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<ConsumableBasic>(json);

            ExternalResponse<ConsumableBasic, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<ConsumableBasic, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<ConsumableBasic, ErrorResponse>
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
                result = new ExternalResponse<ConsumableBasic, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>> GetConsumableMovements(string? identityCenterToken, int id)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{id}/Movements?$top=1000");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<ConsumableMovementBasic>>(json);

            ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result ?? []
                    };
                }
                else
                {
                    result = new ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>
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
                result = new ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

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

        public async Task<ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>> GetConsumableServiceAreas(string? identityCenterToken, int id)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{id}/ServiceAreas");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<List<ConsumableServiceAreaBasic>>(json);

            ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>
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
                result = new ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>
                {
                    Error = new ErrorResponse{
                        Error = "unknown_error",
                        ErrorDescription = "exception_error"
                    }
                };
            }

            return result;
        }

        public async Task<ExternalResponse<dynamic, ErrorResponse>> SetConsumptionInAnalysis(ConsumableConsumptionInAnalysisDTO body)
        {

            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
            var content = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8, "application/json"
            );

            var response = await _httpClient.PostAsync($"{_settings.MyLIMSApiURLBase}/ConsumableMovementApi/ConsumptionInAnalysis", content);

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);

            ExternalResponse<dynamic, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = myLIMSResponse
                        }
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<dynamic, ErrorResponse>
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