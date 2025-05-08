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
    public class SpecificationsController(ISpecificationsServices specificationsServices, ILogger<SpecificationsController> logger) : ControllerBase
    {
        private readonly ISpecificationsServices _specificationsServices = specificationsServices;
        private readonly ILogger<SpecificationsController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<SpecificationBasic>>>> Get()
        {
            var response = await _specificationsServices.GetSpecifications();

            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<SpecificationBasic>>
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