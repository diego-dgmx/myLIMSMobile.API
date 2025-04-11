using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Services {
    public class ConsumablesServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IConsumablesServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<dynamic, ErrorResponse>> CreateConsumableSample(CreateConsumableSampleDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync($"{_settings.MyLIMSApiURLBase}/ConsumableApi/NewConsumableSample", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = myLIMSResponse,
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<dynamic, ErrorResponse>
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

        public async Task<ExternalResponse<ConsumableBasic, ErrorResponse>> GetConsumable(string? identityCenterToken, string? identityCompany, int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{id}");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<ConsumableBasic>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<ConsumableBasic, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<ConsumableBasic, ErrorResponse>
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
                return new ExternalResponse<ConsumableBasic, ErrorResponse>
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

        public async Task<ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>> GetConsumableMovements(string? identityCenterToken, string? identityCompany, int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);
                
                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{id}/Movements?$top=1000");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<ConsumableMovementBasic>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result ?? []
                    };
                }
                else
                {
                    return new ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>> GetConsumables(
            string? identityCenterToken, string? identityCompany, int? top = null, int? skip = null, string? filter = null, string? orderBy = null)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);
                
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

                if(!filter.IsNullOrEmpty())
                {
                    query["$filter"] += $"{filter}";
                }

                if(orderBy != null)
                {
                    query["$orderby"] = $"{orderBy}";
                }

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<ConsumableBasic>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>
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
                return new ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>> GetConsumablesByConsumableTypeId(int consumableTypeId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

                var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Consumables/GetByConsumableTypeIdForMovement?consumableTypeId={consumableTypeId}");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<SimpleConsumableBasic>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>
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
                return new ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>> GetConsumableServiceAreas(string? identityCenterToken, string? identityCompany, int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{id}/ServiceAreas");

                var json = await response.Content.ReadAsStringAsync();
            
                if(response.IsSuccessStatusCode)
                {
                    var myLIMSResponse = JsonConvert.DeserializeObject<List<ConsumableServiceAreaBasic>>(json);
                    return new ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<List<ConsumableServiceCenterBasic>, ErrorResponse>> GetConsumableServiceCenters(string? identityCenterToken, string? identityCompany, int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{id}/ServiceCenters");

                var json = await response.Content.ReadAsStringAsync();
            
                if(response.IsSuccessStatusCode)
                {
                    var myLIMSResponse = JsonConvert.DeserializeObject<List<ConsumableServiceCenterBasic>>(json);
                    return new ExternalResponse<List<ConsumableServiceCenterBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<ConsumableServiceCenterBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ConsumableServiceCenterBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<string, ErrorResponse>> ActivateMovement(
            string? identityCenterToken, string? identityCompany, int consumableId, int movementId, UpdateMovementDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PutAsync(
                    $"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{consumableId}/Movements/{movementId}/Activate", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<string>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    string errorDescriptionPattern = @"LabsoftmyLIMSErrorDescription:\s*(.*)";
                    Match match = Regex.Match(json, errorDescriptionPattern);

                    return new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = match.Groups[1].Value.Trim(),
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<string, ErrorResponse>
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

        public async Task<ExternalResponse<string, ErrorResponse>> InactivateMovement(
            string? identityCenterToken, string? identityCompany, int consumableId, int movementId, UpdateMovementDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PutAsync(
                    $"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables/{consumableId}/Movements/{movementId}/Inactivate", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<string>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    string errorDescriptionPattern = @"LabsoftmyLIMSErrorDescription:\s*(.*)";
                    Match match = Regex.Match(json, errorDescriptionPattern);

                    return new ExternalResponse<string, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = match.Groups[1].Value.Replace("\\\"", "").Replace("\"", "").Trim(),
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<string, ErrorResponse>
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

        public async Task<ExternalResponse<dynamic, ErrorResponse>> SetConsumptionInAnalysis(ConsumableConsumptionInAnalysisDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync($"{_settings.MyLIMSApiURLBase}/ConsumableMovementApi/ConsumptionInAnalysis", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = myLIMSResponse,
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<dynamic, ErrorResponse>
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

        public async Task<ExternalResponse<List<ConsumableInfoBasic>, ErrorResponse>> GetConsumableInfos(int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

                var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Consumables/{id}/infos?&top=1000");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<ConsumableInfoBasic>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<ConsumableInfoBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<ConsumableInfoBasic>, ErrorResponse>
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
                return new ExternalResponse<List<ConsumableInfoBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<dynamic, ErrorResponse>> RemoveStock(UpdateStockDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync($"{_settings.MyLIMSApiURLBase}/ConsumableMovementApi/ConsumptionManual", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = myLIMSResponse,
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<dynamic, ErrorResponse>
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

        public async Task<ExternalResponse<dynamic, ErrorResponse>> AddStock(UpdateStockDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync($"{_settings.MyLIMSApiURLBase}/ConsumableMovementApi/InputStock", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = myLIMSResponse,
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<dynamic, ErrorResponse>
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

        public async Task<ExternalResponse<int, ErrorResponse>> CreateConsumable(string? identityCenterToken, string? identityCompany, ConsumableCreateDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync(
                    $"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<InfoTypeBasic>(json);
                Console.WriteLine(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<int, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Id ?? 0
                    };
                }
                else
                {
                    string errorDescriptionPattern = @"LabsoftmyLIMSErrorDescription:\s*(.*)";
                    Match match = Regex.Match(json, errorDescriptionPattern);

                    return new ExternalResponse<int, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = match.Groups[1].Value.Replace("\\\"", "").Replace("\"", "").Trim(),
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<int, ErrorResponse>
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

        public async Task<ExternalResponse<dynamic, ErrorResponse>> UpdateConsumable(string? identityCenterToken, string? identityCompany, ConsumableUpdateDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PutAsync(
                    $"{_settings.LabsoftMyLIMSApiURLBase}/v1/Consumables", content);

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<dynamic>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    string errorDescriptionPattern = @"LabsoftmyLIMSErrorDescription:\s*(.*)";
                    Match match = Regex.Match(json, errorDescriptionPattern);

                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = match.Groups[1].Value.Replace("\\\"", "").Replace("\"", "").Trim(),
                            Exception = response.StatusCode == HttpStatusCode.InternalServerError ?
                                new Exception(json) : null
                        }
                    };
                }
            }
            catch(Exception ex) {
                return new ExternalResponse<dynamic, ErrorResponse>
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