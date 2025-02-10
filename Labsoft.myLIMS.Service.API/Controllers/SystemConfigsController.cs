using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SystemConfigsController(ISystemConfigsServices systemConfigsServices, ILogger<SystemConfigsController> logger) : ControllerBase
    {
        private readonly ISystemConfigsServices _systemConfigsServices = systemConfigsServices;
        private readonly ILogger<SystemConfigsController> _logger = logger;

        [HttpGet("GetSampleListInfo")]
        public async Task<ActionResult<ResponseBase<SampleListInfo>>> GetSampleListInfo()
        {
            var response = await _systemConfigsServices.GetSampleListInfo();

            if(response.StatusCode == 200)
            {
                return StatusCode(response.StatusCode, new ResponseBase<SampleListInfo>
                {
                    Ok = response.Success != null,
                    Data = response.Success
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
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