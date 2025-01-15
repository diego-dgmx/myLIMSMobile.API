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
    public class FilesController(IFilesServices filesServices) : ControllerBase
    {
        private readonly IFilesServices _filesServices = filesServices;

        [HttpPost("")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ResponseBase<dynamic>>> UploadFile(
            [FromForm] IFormFile file)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _filesServices.UploadFile(identityCenterToken, file);
            
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
    }
}