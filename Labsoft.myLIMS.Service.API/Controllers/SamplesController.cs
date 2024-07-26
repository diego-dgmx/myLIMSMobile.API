using Entities;
using LabsoftAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController(ISamplesServices samplesServices) : ControllerBase
    {
        private readonly ISamplesServices _samplesServices = samplesServices;

        [HttpGet("GetAllMethods")]
        public async Task<IActionResult> GetAllMethods()
        {
            var response = await _samplesServices.GetAllMethods();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<AnalysisMethod>>
                {
                    Ok = response.Success != null,
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

        [HttpGet("GetAllSamples")]
        public async Task<IActionResult> GetAllSamples([FromQuery] int[] sampleIds, [FromQuery] int[] methodIds)
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                if(!sampleIds.IsNullOrEmpty()) {
                    results = results.Where(
                        item => sampleIds.ToList().Contains(item.Sample?.Id ?? 0))
                        .ToList();
                }

                if(!methodIds.IsNullOrEmpty()) {
                    results = results.Where(
                        item => methodIds.ToList().Contains(item.Method?.MasterId ?? 0))
                        .ToList();
                }

                return StatusCode(response.StatusCode, new ResponseBase<AnalyticsSamples>
                {
                    Ok = response.Success != null,
                    Data = new AnalyticsSamples
                    {
                        PrepareAnalytics = results.Where(item =>
                            item?.CurrentStatus?.MethodStatus?.Id == 1)
                            .Select(item => item.Sample!).ToList(),
                        CarriedOutAnalytics = results.Where(item =>
                            item?.CurrentStatus?.MethodStatus?.Id == 3)
                            .Select(item => item.Sample!).ToList(),
                        ReviewAnalytics = results.Where(item =>
                            item?.CurrentStatus?.MethodStatus?.Id == 2)
                            .Select(item => item.Sample!).ToList(),
                        BatchQC = results.SelectMany(item => item.QCTests!)
                            .Select(subItem => subItem.QCTest).ToList()
                    }
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