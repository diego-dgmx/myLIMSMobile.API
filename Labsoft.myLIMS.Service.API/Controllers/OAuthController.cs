using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Labsoft.myLIMS.Service.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly System.Net.Http.IHttpClientFactory _httpClientFactory;

        public OAuthController(IConfiguration configuration, System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("get-data")]
        public async Task<IActionResult> GetData(string username, string password)
        {
            var clientId = _configuration["OAuth:ClientId"];
            var tokenUrl = _configuration["OAuth:TokenUrl"];
            var scope = _configuration["OAuth:Scope"];

            var client = _httpClientFactory.CreateClient();

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
            var requestBody = new StringContent(
                $"grant_type=password&client_id={clientId}&username={username}&password={password}&scope={scope}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            tokenRequest.Content = requestBody;

            var tokenResponse = await client.SendAsync(tokenRequest);
            if (!tokenResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)tokenResponse.StatusCode, await tokenResponse.Content.ReadAsStringAsync());
            }

            var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<TokenResponse>(tokenResponseContent);
            var token = tokenData.access_token;

            var result = new ApiResponse<string>
            {
                Data = token,
                Message = "Acceso Autenticado Correctamente",
                StatusCode = 200
            };

            return Ok(result);
        }
        private class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string access_token { get; set; }
        }


        public class ApiResponse<T>
        {
            public T Data { get; set; }
            public string Message { get; set; }
            public int StatusCode { get; set; }
        }


    }

}
