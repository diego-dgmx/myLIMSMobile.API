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
    public class EquipmentsController(IEquipmentsServices equipmentsServices, ILogger<EquipmentsController> logger) : ControllerBase
    {
        private readonly IEquipmentsServices _equipmentsServices = equipmentsServices;
        private readonly ILogger<EquipmentsController> _logger = logger;

        [HttpGet("{equipmentTypeId}/ByEquipmentTypeId")]
        public async Task<ActionResult<ResponseBase<List<Equipment>>>> ByEquipmentTypeId(int equipmentTypeId)
        {
            var response = await _equipmentsServices.GetEquipmentsByEquipmentTypeId(equipmentTypeId);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<Equipment>>
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

        [HttpGet("ByServiceCenters")]
        public async Task<ActionResult<ResponseBase<List<Equipment>>>> ByServiceCenters()
        {
            var response = await _equipmentsServices.GetEquipmentsByServiceCenters();

            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<Equipment>>
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