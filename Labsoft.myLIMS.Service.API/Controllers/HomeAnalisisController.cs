using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Labsoft.myLIMS.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeAnalisisController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeAnalisisController(IHttpClientFactory _clientFactory)
        {
            this._clientFactory = _clientFactory;
        }

        [HttpGet("get-analisis")]
        public async Task<IActionResult> GetAnalisis()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://produto.mylimsweb.cloud/api/v2/samples/GetAllMethodsForPerformTask");

            request.Headers.Add("x-access-key", "75a57345456abe22f9972a82973b3e3b");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseContent);

                // Initialize counts
                int prepararCount = 0;
                int realizarCount = 0;
                int revisarCount = 0;
                int cqCount = 0;

                // Parse the 'Result' array
                var results = responseObject["Result"];

                // Count occurrences for each analysis status
                foreach (var item in results)
                {
                    var status = int.Parse(item["CurrentStatus"]["MethodStatus"]["MethodStatusBehaviorId"].ToString());
                    if (status == 1)
                    {
                        prepararCount++;
                    }
                    else if (status == 2)
                    {
                        realizarCount++;
                    }
                    else if (status == 3)
                    {
                        revisarCount++;
                    }
                    else if (status == 4)
                    {
                        cqCount++;
                    }
                }

                // Return the counts as JSON
                return Ok(new
                {
                    AnalisisAPreparar = prepararCount,
                    AnalisisARealizar = realizarCount,
                    AnalisisARevisar = revisarCount,
                    BatallaCQ = cqCount
                });
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
    
    }
}
