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
    public class ServiceAreasController(IServiceAreasServices serviceAreasServices, ILogger<ServiceAreasController> logger) : ControllerBase
    {
        private readonly IServiceAreasServices _serviceAreasServices = serviceAreasServices;
        private readonly ILogger<ServiceAreasController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<ServiceAreaBasic>>>> Get()
        {
            var response = await _serviceAreasServices.GetServiceAreas();

            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<ServiceAreaBasic>>
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