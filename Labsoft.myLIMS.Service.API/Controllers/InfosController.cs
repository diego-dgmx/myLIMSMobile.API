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
    public class InfosController(IInfosServices infosServices, ILogger<InfosController> logger) : ControllerBase
    {
        private readonly IInfosServices _infosServices = infosServices;
        private readonly ILogger<InfosController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult<ResponseBase<InfoBasic>>> Get(
            [FromQuery] int[] infoIds,
            [FromQuery] string[] identifications,
            [FromQuery] int perPage = 10,
            [FromQuery] int page = 1)
        {
            string filter = "";

            if(!infoIds.IsNullOrEmpty()) {
                List<string> values = [];
                foreach(int id in infoIds) {
                    values.Add($"Id eq {id}");
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

            var response = await _infosServices.GetInfos(perPage, (page - 1) * perPage, filter);

            if(response.StatusCode == 200)
            {
                return StatusCode(response.StatusCode, new ResponseBase<Pagination<InfoBasic>>
                {
                    Data = new Pagination<InfoBasic>
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
    }
}