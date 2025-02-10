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
    public class MeasurementUnitsController(IMeasurementUnitsServices measurementUnitsServices, ILogger<MeasurementUnitsController> logger) : ControllerBase
    {
        private readonly IMeasurementUnitsServices _measurementUnitsServices = measurementUnitsServices;
        private readonly ILogger<MeasurementUnitsController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<MeasurementUnitBasic>>>> Get()
        {
            var response = await _measurementUnitsServices.GetMeasurementUnits();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<MeasurementUnitBasic>>
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