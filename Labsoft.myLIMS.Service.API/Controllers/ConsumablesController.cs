using Entities;
using Interfaces;
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
    public class ConsumablesController(IConsumablesServices consumablesServices, ILogger<ConsumablesController> logger) : ControllerBase
    {
        private readonly IConsumablesServices _consumablesServices = consumablesServices;
        private readonly ILogger<ConsumablesController> _logger = logger;

        private enum ConsumableSortParam
        {
            identification,
            consumableType,
            expires
        }

        [HttpGet("")]
        public async Task<ActionResult<ResponseBase<Pagination<ConsumableBasic>>>> GetConsumables(
            [FromQuery] string? sortParam,
            [FromQuery] string[] consumableIdentifications,
            [FromQuery] int[] consumableTypeIds,
            [FromQuery] int perPage = 10,
            [FromQuery] int page = 1)
        {
            if (Enum.TryParse<ConsumableSortParam>(sortParam, out var param)) {
                switch(param) {
                    case ConsumableSortParam.identification:
                        sortParam = "Identification";
                        break;
                    case ConsumableSortParam.consumableType:
                        sortParam = "ConsumableType/Id";
                        break;
                    case ConsumableSortParam.expires:
                        sortParam = "Expires";
                        break;
                }
            }

            string filter = "";

            if(!consumableIdentifications.IsNullOrEmpty()) {
                List<string> values = [];
                foreach(string identification in consumableIdentifications) {
                    values.Add($"contains(Identification,'{identification}')");
                }

                filter += $"({string.Join(" or ", values)}) and ";
            }

            if(!consumableTypeIds.IsNullOrEmpty()) {
                List<string> values = [];
                foreach(int id in consumableTypeIds) {
                    values.Add($"ConsumableType/Id eq {id}");
                }

                filter += $"({string.Join(" or ", values)}) and ";
            }

            if(filter.Length > 0) {
                filter = filter[..^5];
            }

            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.GetConsumables(identityCenterToken, identityCompany, perPage, (page - 1) * perPage, filter, sortParam);
            
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

        [HttpPost("CreateConsumableSample")]
        public async Task<ActionResult<ResponseBase<dynamic>>> CreateConsumableSample(
            [FromBody] CreateConsumableSampleDTO body)
        {
            var response = await _consumablesServices.CreateConsumableSample(body);
            
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseBase<ConsumableBasic>>> GetConsumable(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.GetConsumable(identityCenterToken, identityCompany, id);
            
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

        [HttpGet("{id}/Movements")]
        public async Task<ActionResult<ResponseBase<List<ConsumableMovementBasic>>>> GetConsumableMovements(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.GetConsumableMovements(identityCenterToken, identityCompany, id);
            
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

        [HttpGet("{id}/ServiceAreas")]
        public async Task<ActionResult<ResponseBase<List<ConsumableServiceAreaBasic>>>> GetConsumableServiceAreas(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.GetConsumableServiceAreas(identityCenterToken, identityCompany, id);
            
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

        [HttpGet("{id}/ServiceCenters")]
        public async Task<ActionResult<ResponseBase<List<ConsumableServiceCenterBasic>>>> GetConsumableServiceCenters(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.GetConsumableServiceCenters(identityCenterToken, identityCompany, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<List<ConsumableServiceCenterBasic>>
                {
                    Data = result
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

        [HttpGet("{id}/Infos")]
        public async Task<ActionResult<ResponseBase<List<ConsumableInfoBasic>>>> GetConsumableInfos(int id)
        {
            var response = await _consumablesServices.GetConsumableInfos(id);
            
            if(response.StatusCode == 200)
            {
                return StatusCode(response.StatusCode, new ResponseBase<List<ConsumableInfoBasic>>
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

        [HttpGet("{consumableTypeId}/ByConsumableTypeId")]
        public async Task<ActionResult<ResponseBase<List<SimpleConsumableBasic>>>> ByConsumableTypeId(int consumableTypeId)
        {
            var response = await _consumablesServices.GetAvailableConsumablesByConsumableTypeId(consumableTypeId);
            
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

        [HttpPost("RemoveStock")]
        public async Task<ActionResult<ResponseBase<dynamic>>> RemoveStock(
            [FromBody] UpdateStockDTO body)
        {
            var response = await _consumablesServices.RemoveStock(body);
            
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

        [HttpPost("AddStock")]
        public async Task<ActionResult<ResponseBase<dynamic>>> AddStock(
            [FromBody] UpdateStockDTO body)
        {
            var response = await _consumablesServices.AddStock(body);
            
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

        [HttpPut("{consumableId}/Movements/{movementId}/Activate")]
        public async Task<ActionResult<ResponseBase<string>>> ActivateMovement(
            int consumableId, int movementId, [FromBody] UpdateMovementDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.ActivateMovement(identityCenterToken, identityCompany, consumableId, movementId, body);
            
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

        [HttpPut("{consumableId}/Movements/{movementId}/Inactivate")]
        public async Task<ActionResult<ResponseBase<string>>> InactivateMovement(
            int consumableId, int movementId, [FromBody] UpdateMovementDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.InactivateMovement(identityCenterToken, identityCompany, consumableId, movementId, body);
            
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

        [HttpPost]
        public async Task<ActionResult<ResponseBase<int>>> CreateConsumable([FromBody] ConsumableCreateDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.CreateConsumable(identityCenterToken, identityCompany, body);
            
            if(response.StatusCode == 201)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<int>
                {
                    Data = result
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<int>
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

        [HttpPut]
        public async Task<ActionResult<ResponseBase<dynamic>>> UpdateConsumable([FromBody] ConsumableUpdateDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _consumablesServices.UpdateConsumable(identityCenterToken, identityCompany, body);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Data = result
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

        [HttpGet("Available/{consumableTypeId}/ByConsumableTypeId")]
        public async Task<ActionResult<ResponseBase<List<SimpleConsumableBasic>>>> AvailableConsumablesByConsumableTypeId(int consumableTypeId)
        {
            var response = await _consumablesServices.GetAvailableConsumablesByConsumableTypeId(consumableTypeId);
            
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