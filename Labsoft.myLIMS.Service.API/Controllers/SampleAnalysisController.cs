using Entities;
using LabsoftAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SampleAnalysisController(ISampleAnalysisServices sampleAnalysisServices) : ControllerBase
    {
        private readonly ISampleAnalysisServices _sampleAnalysisServices = sampleAnalysisServices;

        [HttpGet("GetRevisions")]
        public async Task<ActionResult<ResponseBase<List<SampleRevisionBasic>>>> GetRevisions([FromQuery] int[] sampleAnalysisIds)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _sampleAnalysisServices.GetRevisions(identityCenterToken, sampleAnalysisIds);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<List<SampleRevisionBasic>>
                {
                    Data = results
                });
            }
            else
            {
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }
    }
}