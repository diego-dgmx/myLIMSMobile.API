using Entities;
using LabsoftAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConsumablesController(IConsumablesServices consumablesServices) : ControllerBase
    {
        private readonly IConsumablesServices _consumablesServices = consumablesServices;

        [HttpGet("{consumableTypeId}/ByConsumableTypeId")]
        public async Task<ActionResult<ResponseBase<List<Consumable>>>> ByConsumableTypeId(int consumableTypeId)
        {
            var response = await _consumablesServices.GetConsumablesByConsumableTypeId(consumableTypeId);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<Consumable>>
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