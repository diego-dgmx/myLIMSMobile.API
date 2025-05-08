using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labsoft.myLIMS.Service.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InterventionTypesController(IInterventionTypesServices interventionTypesServices, ILogger<InterventionTypesController> logger) : ControllerBase
    {
        private readonly IInterventionTypesServices _interventionTypesServices = interventionTypesServices;
        private readonly ILogger<InterventionTypesController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<InterventionType>>>> Get()
        {
            var response = await _interventionTypesServices.GetInterventionTypes();

            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<InterventionType>>
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