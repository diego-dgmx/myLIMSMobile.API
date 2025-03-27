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
    public class ConsumableTypesController(IConsumableTypesServices consumableTypesServices, ILogger<ConsumableTypesController> logger) : ControllerBase
    {
        private readonly IConsumableTypesServices _consumableTypesServices = consumableTypesServices;
        private readonly ILogger<ConsumableTypesController> _logger = logger;

        [HttpGet("")]
        public async Task<ActionResult<ResponseBase<List<ConsumableTypeBasic>>>> GetConsumableTypes()
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumableTypesServices.GetConsumableTypes(identityCenterToken, identityCompany);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<List<ConsumableTypeBasic>>
                {
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