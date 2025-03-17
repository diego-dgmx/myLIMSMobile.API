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
    public class EquipmentTypesController(IEquipmentTypesServices equipmentTypesServices, ILogger<EquipmentTypesController> logger) : ControllerBase
    {
        private readonly IEquipmentTypesServices _equipmentTypesServices = equipmentTypesServices;
        private readonly ILogger<EquipmentTypesController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<ResponseBase<EquipmentType>>> Get()
        {
            var response = await _equipmentTypesServices.GetEquipmentTypes();

            if(response.StatusCode == 200)
            {
                return StatusCode(response.StatusCode, new ResponseBase<List<EquipmentType>>
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