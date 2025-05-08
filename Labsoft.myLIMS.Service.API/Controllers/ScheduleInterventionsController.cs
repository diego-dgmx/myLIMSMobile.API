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
    public class ScheduleInterventionsController(IScheduleInterventionsServices scheduleInterventionsServices, ILogger<ScheduleInterventionsController> logger) : ControllerBase
    {
        private readonly IScheduleInterventionsServices _scheduleInterventionsServices = scheduleInterventionsServices;
        private readonly ILogger<ScheduleInterventionsController> _logger = logger;

        private enum ScheduleInterventionsSortParam
        {
            equipmentType,
            identification,
            nextIntervention
        }

        [HttpGet]
        public async Task<ActionResult<ResponseBase<Pagination<ScheduleInterventionBasic>>>> Get(
            [FromQuery] string? sortParam,
            [FromQuery] int[] equipmentTypeIds,
            [FromQuery] string[] identifications,
            [FromQuery] int perPage = 10,
            [FromQuery] int page = 1)
        {
            if (Enum.TryParse<ScheduleInterventionsSortParam>(sortParam, out var param)) {
                switch(param) {
                    case ScheduleInterventionsSortParam.equipmentType:
                        sortParam = "Equipment/EquipmentType/Identification";
                        break;
                    case ScheduleInterventionsSortParam.identification:
                        sortParam = "Identification";
                        break;
                    case ScheduleInterventionsSortParam.nextIntervention:
                        sortParam = "NextIntervention";
                        break;
                }
            }

            string filter = "";

            if(!equipmentTypeIds.IsNullOrEmpty()) {
                List<string> values = [];
                foreach(int id in equipmentTypeIds) {
                    values.Add($"Equipment/EquipmentType/Id eq {id}");
                }

                filter += $"({string.Join(" or ", values)}) and ";
            }

            if(!identifications.IsNullOrEmpty()) {
                List<string> values = [];
                foreach(string identification in identifications) {
                    values.Add($"substringof('{identification}', Identification)");
                }

                filter += $"({string.Join(" or ", values)}) and ";
            }

            if(filter.Length > 0) {
                filter = filter[..^5];
            }

            var response = await _scheduleInterventionsServices.GetScheduleInterventions(sortParam, filter,  perPage, (page - 1) * perPage);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<Pagination<ScheduleInterventionBasic>>
                {
                    Data = new Pagination<ScheduleInterventionBasic>
                    {
                        CurrentPage = page,
                        PerPage = perPage,
                        TotalPages = (int) Math.Ceiling((double) (response.Success?.TotalCount ?? 0) / perPage),
                        TotalItems = response.Success?.TotalCount ?? 0,
                        Items = results
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseBase<ScheduleInterventionBasic>>> GetScheduleIntervention(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.GetScheduleIntervention(identityCenterToken, identityCompany, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<ScheduleInterventionBasic>
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

        [HttpGet("{id}/Specifications")]
        public async Task<ActionResult<ResponseBase<List<ScheduleSpecificationBasic>>>> GetScheduleSpecifications(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.GetScheduleSpecifications(identityCenterToken, identityCompany, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<List<ScheduleSpecificationBasic>>
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

        [HttpGet("{id}/ControlPlan")]
        public async Task<ActionResult<ResponseBase<ScheduleControlPlanBasic>>> GetScheduleControlPlan(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.GetScheduleControlPlan(identityCenterToken, identityCompany, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<ScheduleControlPlanBasic>
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

        [HttpGet("{id}/AnalysisGroups")]
        public async Task<ActionResult<ResponseBase<List<ScheduleAnalysisGroupBasic>>>> GetScheduleAnalysisGroups(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.GetScheduleAnalysisGroups(identityCenterToken, identityCompany, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<List<ScheduleAnalysisGroupBasic>>
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

        [HttpGet("{id}/Interventions")]
        public async Task<ActionResult<ResponseBase<List<ScheduleSampleInterventionBasic>>>> GetScheduleSampleInterventions(int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.GetScheduleSampleInterventions(identityCenterToken, identityCompany, id);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<List<ScheduleSampleInterventionBasic>>
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
        public async Task<ActionResult<ResponseBase<int>>> CreateScheduleIntervention([FromBody] ScheduleInterventionCreateDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.CreateScheduleIntervention(identityCenterToken, identityCompany, body);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(201, new ResponseBase<int>
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
        public async Task<ActionResult<ResponseBase<dynamic>>> UpdateScheduleIntervention([FromBody] ScheduleInterventionUpdateDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.UpdateScheduleIntervention(identityCenterToken, identityCompany, body);

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

        [HttpPost("ScheduleInterventionRoutine")]
        public async Task<ActionResult<ResponseBase<int>>> CreateScheduleInterventionRoutine([FromBody] ScheduleInterventionDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.CreateScheduleInterventionRoutine(identityCenterToken, identityCompany, body);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(201, new ResponseBase<int>
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

        [HttpPost("ScheduleInterventionExtraordinary")]
        public async Task<ActionResult<ResponseBase<int>>> CreateScheduleInterventionExtraordinary([FromBody] ScheduleInterventionDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _scheduleInterventionsServices.CreateScheduleInterventionExtraordinary(identityCenterToken, identityCompany, body);

            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(201, new ResponseBase<int>
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
    }
}