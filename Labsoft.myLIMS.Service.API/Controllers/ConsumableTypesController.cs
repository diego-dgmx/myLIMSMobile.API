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
    public class ConsumableTypesController(IConsumableTypesServices consumableTypesServices) : ControllerBase
    {
        private readonly IConsumableTypesServices _consumableTypesServices = consumableTypesServices;

        [HttpGet("")]
        public async Task<ActionResult<ResponseBase<List<ConsumableTypeBasic>>>> GetConsumableTypes()
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _consumableTypesServices.GetConsumableTypes(identityCenterToken);
            
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