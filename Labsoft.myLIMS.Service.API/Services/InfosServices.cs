using System.Net;
using System.Web;
using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Services {
    public class InfosServices(HttpClient httpClient, IOptions<ApiSettings> settings) : IInfosServices
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ApiSettings _settings = settings.Value;

        public async Task<ExternalResponse<MyLIMSResponseBase<InfoBasic>, ErrorResponse>> GetInfos(int? top = null, int? skip = null, string? filter = null)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/v2/Infos");
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["$inlinecount"] = "allpages";

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

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<InfoBasic>>(json);

                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<MyLIMSResponseBase<InfoBasic>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<MyLIMSResponseBase<InfoBasic>, ErrorResponse>
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
                return new ExternalResponse<MyLIMSResponseBase<InfoBasic>, ErrorResponse>
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