using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesController(IFilesServices filesServices, ILogger<FilesController> logger) : ControllerBase
    {
        private readonly IFilesServices _filesServices = filesServices;
        private readonly ILogger<FilesController> _logger = logger;

        [HttpGet("{fileId}/GetFileData")]
        public async Task<ActionResult<ResponseBase<string>>> GetFileData(int fileId)
        {
            var response = await _filesServices.GetFileData(fileId);
            
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

        [HttpPost("")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ResponseBase<dynamic>>> UploadFile(
            [FromForm] IFormFile file)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _filesServices.UploadFile(identityCenterToken, identityCompany, file);
            
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
    }
}