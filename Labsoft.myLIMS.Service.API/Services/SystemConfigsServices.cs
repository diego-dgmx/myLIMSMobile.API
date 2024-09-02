using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class SystemConfigsServices(HttpClient httpClient, IOptions<ApiSettings> settings) : ISystemConfigsServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<SampleListInfo, ErrorResponse>> GetSampleListInfo()
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
            var response = await _httpClient.GetAsync(
                $"{_settings.MyLIMSApiURLBase}/SystemConfigs/GetSampleListInfo");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<SampleListInfo>(json);

            ExternalResponse<SampleListInfo, ErrorResponse> result;

            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<SampleListInfo, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<SampleListInfo, ErrorResponse>
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
                result = new ExternalResponse<SampleListInfo, ErrorResponse>
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