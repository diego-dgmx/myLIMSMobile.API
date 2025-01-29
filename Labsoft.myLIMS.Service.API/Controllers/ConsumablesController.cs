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

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseBase<ConsumableBasic>>> GetConsumable(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _consumablesServices.GetConsumable(identityCenterToken, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<ConsumableBasic>
                {
                    Data = result
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

        [HttpGet("{id}/Movements")]
        public async Task<ActionResult<ResponseBase<List<ConsumableMovementBasic>>>> GetConsumableMovements(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _consumablesServices.GetConsumableMovements(identityCenterToken, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<List<ConsumableMovementBasic>>
                {
                    Data = result
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

        [HttpGet("{id}/ServiceAreas")]
        public async Task<ActionResult<ResponseBase<List<ConsumableServiceAreaBasic>>>> GetConsumableServiceAreas(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _consumablesServices.GetConsumableServiceAreas(identityCenterToken, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<List<ConsumableServiceAreaBasic>>
                {
                    Data = result
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

        [HttpPost("SetConsumptionInAnalysis")]
        public async Task<ActionResult<ResponseBase<dynamic>>> SetConsumptionInAnalysis(
            [FromBody] ConsumableConsumptionInAnalysisDTO body)
        {
            var response = await _consumablesServices.SetConsumptionInAnalysis(body);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
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

        [HttpPut("{consumableId}/Movements/{movementId}/Inactivate")]
        public async Task<ActionResult<ResponseBase<string>>> SetConsumptionInAnalysis(
            int consumableId, int movementId, [FromBody] InactivateMovementDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _consumablesServices.InactivateMovement(identityCenterToken, consumableId, movementId, body);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<string>
                {
                    Data = result
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