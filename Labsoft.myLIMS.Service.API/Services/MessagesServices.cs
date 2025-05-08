using System.Net;
using System.Text;
using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class MessagesServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IMessagesServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<MessageBasic>, ErrorResponse>> GetMessagesBySampleId(int sampleId)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

                var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/MessageApi/GetMessagesBySampleId?sampleId={sampleId}");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBasic<List<MessageBasic>>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<MessageBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Items
                    };
                }
                else
                {
                    return new ExternalResponse<List<MessageBasic>, ErrorResponse>
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
                return new ExternalResponse<List<MessageBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<List<MessageTypeBasic>, ErrorResponse>> GetMessageTypes()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

                var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/v2/Messages/GetMessageTypes?$top=1000");

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<MessageTypeBasic>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<MessageTypeBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    return new ExternalResponse<List<MessageTypeBasic>, ErrorResponse>
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
                return new ExternalResponse<List<MessageTypeBasic>, ErrorResponse>
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

        public async Task<ExternalResponse<dynamic, ErrorResponse>> SendMessage(
            string? identityCenterToken, string? identityCompany, SendMessageWithEntitiesAttachedDTO body)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"
                );

                var response = await _httpClient.PostAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/Messages/SendMessageWithEntitiesAttached", content);

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
                    return new ExternalResponse<dynamic, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
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
    }
}