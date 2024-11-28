using System.Text;
using System.Web;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class MethodsServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IMethodsServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<MethodPrerequisiteAnalysisBasic>, ErrorResponse>> MethodPrerequisiteAnalysis(int methodId)
        {
            _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);

            var response = await _httpClient.GetAsync($"{_settings.MyLIMSApiURLBase}/Methods/{methodId}/MethodPrerequisiteAnalysis");

            var json = await response.Content.ReadAsStringAsync();
            var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<MethodPrerequisiteAnalysisBasic>>(json);

            ExternalResponse<List<MethodPrerequisiteAnalysisBasic>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<MethodPrerequisiteAnalysisBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse?.Result
                    };
                }
                else
                {
                    result = new ExternalResponse<List<MethodPrerequisiteAnalysisBasic>, ErrorResponse>
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
                result = new ExternalResponse<List<MethodPrerequisiteAnalysisBasic>, ErrorResponse>
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