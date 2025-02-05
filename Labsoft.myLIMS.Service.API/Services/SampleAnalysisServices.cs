using System.Net.Http.Headers;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class SampleAnalysisServices(HttpClient httpClient, IOptions<ApiSettings> settings) : ISampleAnalysisServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>> GetRevisions(string? identityCenterToken, int[] sampleAnalysisIds)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
            var queryParams = sampleAnalysisIds.Select((value, index) => $"SampleAnalysisIds={value}").ToArray();

            var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/SampleAnalysis/Revision?{string.Join("&", queryParams)}");

            var json = await response.Content.ReadAsStringAsync(); 
            var myLIMSResponse = JsonConvert.DeserializeObject<List<SampleRevisionBasic>>(json);

            ExternalResponse<List<SampleRevisionBasic>, ErrorResponse> result;
            
            try
            {
                if(response.IsSuccessStatusCode)
                {
                    result = new ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    result = new ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>
                    {
                        StatusCode = 400,
                        Error = new ErrorResponse
                        {
                            Error = "external_request_error",
                            ErrorDescription = "request_error"
                        }
                    };
                }
            }
            catch(Exception) {
                result = new ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>
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