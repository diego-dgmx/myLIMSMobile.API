using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceCentersController(IServiceCentersServices serviceCentersServices, ILogger<ServiceCentersController> logger) : ControllerBase
    {
        private readonly IServiceCentersServices _serviceCentersServices = serviceCentersServices;
        private readonly ILogger<ServiceCentersController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<ServiceCenterBasic>>>> Get()
        {
            var response = await _serviceCentersServices.GetServiceCenters();

            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<ServiceCenterBasic>>
                {
                    Data = results
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