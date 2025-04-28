using System.Net;
using System.Web;
using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services
{
    public class AccountsService : IAccountsService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _settings;

        public AccountsService(HttpClient httpClient, IOptions<ApiSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<ExternalResponse<MyLIMSResponseBase<Account>, ErrorResponse>> GetAccounts(int? accountType)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("x-access-key", _settings.MyLIMSApiAccessKey);
                var uriBuilder = new UriBuilder($"{_settings.MyLIMSApiURLBase}/v2/Accounts");
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["$inlinecount"] = "allpages";

                if(accountType != null)
                {
                    query["$accountType"] = $"{accountType}";
                }

                uriBuilder.Query = query.ToString();

                var response = await _httpClient.GetAsync(uriBuilder.ToString());

                var json = await response.Content.ReadAsStringAsync();
                var myLIMSResponse = JsonConvert.DeserializeObject<MyLIMSResponseBase<Account>>(json);

                if(response.IsSuccessStatusCode)
                {
                    return new ExternalResponse<MyLIMSResponseBase<Account>, ErrorResponse>
                    {
                        StatusCode = (int) response.StatusCode,
                        Success = myLIMSResponse
                    };
                }
                else
                {
                    return new ExternalResponse<MyLIMSResponseBase<Account>, ErrorResponse>
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
                return new ExternalResponse<MyLIMSResponseBase<Account>, ErrorResponse>
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
