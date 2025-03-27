using System.Net;
using Entities;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services {
    public class SampleAnalysisServices(HttpClient httpClient, IOptions<ApiSettings> settings) : ISampleAnalysisServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>> GetRevisions(string? identityCenterToken, string? identityCompany, int[] sampleAnalysisIds)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + identityCenterToken);
                _httpClient.DefaultRequestHeaders.Add("company", identityCompany);

                var queryParams = sampleAnalysisIds.Select((value, index) => $"SampleAnalysisIds={value}").ToArray();

                var response = await _httpClient.GetAsync($"{_settings.LabsoftMyLIMSApiURLBase}/v1/SampleAnalysis/Revision?{string.Join("&", queryParams)}");

                var json = await response.Content.ReadAsStringAsync(); 
                var myLIMSResponse = JsonConvert.DeserializeObject<List<SampleRevisionBasic>>(json);
            
                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>
                    {
                        StatusCode = 200,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>
                    {
                        StatusCode = 400,
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
                return new ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>
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