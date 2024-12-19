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
    public class ConsumablesController(IConsumablesServices consumablesServices) : ControllerBase
    {
        private readonly IConsumablesServices _consumablesServices = consumablesServices;

        [HttpGet("")]
        public async Task<ActionResult<ResponseBase<Pagination<ConsumableBasic>>>> GetConsumables(
            [FromQuery] string? sortParam,
            [FromQuery] int perPage = 10,
            [FromQuery] int page = 1)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _consumablesServices.GetConsumables(identityCenterToken, perPage, (page - 1) * perPage, sortParam);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<Pagination<ConsumableBasic>>
                {
                    Data = new Pagination<ConsumableBasic>
                    {
                        CurrentPage = page,
                        PerPage = perPage,
                        TotalPages = (int) Math.Ceiling((double) (response.Success?.TotalCount ?? 0) / perPage),
                        TotalItems = response.Success?.TotalCount ?? 0,
                        Items = response.Success?.Result ?? []
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

        [HttpGet("{consumableTypeId}/ByConsumableTypeId")]
        public async Task<ActionResult<ResponseBase<List<SimpleConsumableBasic>>>> ByConsumableTypeId(int consumableTypeId)
        {
            var response = await _consumablesServices.GetConsumablesByConsumableTypeId(consumableTypeId);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<SimpleConsumableBasic>>
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