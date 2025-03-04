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
            scheduleInterventionIdentification
        }

        [HttpGet]
        public async Task<ActionResult<ResponseBase<Pagination<ScheduleInterventionBasic>>>> Get(
            [FromQuery] string? sortParam,
            [FromQuery] int[] equipmentTypeIds,
            [FromQuery] string[] scheduleInterventionIdentifications,
            [FromQuery] int perPage = 10,
            [FromQuery] int page = 1)
        {
            if (Enum.TryParse<ScheduleInterventionsSortParam>(sortParam, out var param)) {
                switch(param) {
                    case ScheduleInterventionsSortParam.equipmentType:
                        sortParam = "Equipment/EquipmentType/Identification";
                        break;
                    case ScheduleInterventionsSortParam.scheduleInterventionIdentification:
                        sortParam = "Identification";
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

            if(!scheduleInterventionIdentifications.IsNullOrEmpty()) {
                List<string> values = [];
                foreach(string identification in scheduleInterventionIdentifications) {
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
    }
}